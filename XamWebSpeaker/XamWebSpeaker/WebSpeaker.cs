using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.TextToSpeech;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Abstractions.Enumerations;
using Xamarin.Forms;

using Xamarin.Essentials;

namespace XamWebSpeaker
{
    // https://docs.microsoft.com/en-us/xamarin/essentials/text-to-speech
    // https://github.com/jamesmontemagno/TextToSpeechPlugin
    // 以上两个库都支持朗读，但essentials不支持语速设置
    public class WebSpeaker
    {
        public static WebSpeaker Instance { get; } = new WebSpeaker();

        protected WebSpeaker()
        {
            m_SpeakRate = SpeakRate;
            m_Volume = Volume;
            m_Pitch = Pitch;
            m_IsScreenLock = IsScreenLock;
        }

        #region tts
        private double m_SpeakRate;
        public double SpeakRate
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                    m_SpeakRate = Preferences.Get("SpeakRate", 1.7);
                else
                    m_SpeakRate = Preferences.Get("SpeakRate", 0.6);
                return m_SpeakRate;
            }
            set
            {
                m_SpeakRate = Math.Round(value, 2);
                Preferences.Set("SpeakRate", m_SpeakRate);
            }
        }

        public double MaxSpeakRate
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                    return 2.0;
                return 1.0;
            }
        }

        public double MinSpeakRate
        {
            get
            {
                return 0.0;
            }
        }

        private double m_Volume;
        public double Volume
        {
            get
            {
                m_Volume = Preferences.Get("Volume", 1.0);
                return m_Volume;
            }
            set
            {
                m_Volume = Math.Round(value, 1);
                Preferences.Set("Volume", m_Volume);
            }
        }

        private double m_Pitch;
        public double Pitch
        {
            get
            {
                m_Pitch = Preferences.Get("Pitch", 1.0);
                return m_Pitch;
            }
            set
            {
                m_Pitch = Math.Round(value, 1);
                Preferences.Set("Pitch", m_Pitch);
            }
        }
        #endregion

        #region settings
        private bool m_IsScreenLock;
        public bool IsScreenLock
        {
            get
            {
                m_IsScreenLock = Preferences.Get(typeof(ScreenLock).Name, false);
                return m_IsScreenLock;
            }
            set
            {
                Preferences.Set(typeof(ScreenLock).Name, value);
                m_IsScreenLock = value;
            }
        }
        #endregion

        bool isSpeakingPage;
        bool isSpeakingPart;
        bool isCancelling;

        CancellationTokenSource cts;

        private FormsWebView webView;

        public void SetWebView(FormsWebView webView)
        {
            this.webView = webView;
        }

        private string GetJavaScriptString()
        {
            // 为了测试，可以考虑获取脚本
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/files?tabs=vswin
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;

            string text = "";
            {
                // 调用不同的平台的特殊代码
                Stream stream = assembly.GetManifestResourceStream($"XamWebSpeaker.bell_pepper_info_{Device.RuntimePlatform}.js");

                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }

            {
                Stream stream = assembly.GetManifestResourceStream("XamWebSpeaker.bell_pepper_info.js");

                using (var reader = new System.IO.StreamReader(stream))
                {
                    text += reader.ReadToEnd();
                }
            }

            return text;
        }

        private void DoCmd(string cmd)
        {
            // 将会被多次调用，这样写，按需加载
            // 由于还没有注入，所以分平台写
            string url = "";
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    url = "(typeof(gBellPepperInfo) == 'undefined' ? bell_pepper_info.InitRun('gBellPepperInfo." + cmd + "') : gBellPepperInfo." + cmd + " )";
                    break;
                case Device.iOS:
                    url = "(typeof(gBellPepperInfo) == 'undefined' ? window.webkit.messageHandlers.InitRun.postMessage('gBellPepperInfo." + cmd + "') : gBellPepperInfo." + cmd + " )";
                    break;
            }
            webView.InjectJavascriptAsync(url).GetAwaiter();
        }

        private void StopSpeak()
        {
            isSpeakingPage = false;
            isSpeakingPart = false;
            Cancel();
        }

        public void StartPage()
        {
            StopSpeak();
            DoCmd("StartPage()");
        }

        public void Pause()
        {
            // 停止时取消常亮，不用管是否真的启用
            //ScreenLock.RequestRelease();

            StopSpeak();
            DoCmd("StopTimer()");
        }

        public void Play()
        {
            isSpeakingPage = true;
            isSpeakingPart = true;

            // 感觉在小米上不管用啊
            //if (m_IsScreenLock)
            //{
            //    if (!ScreenLock.IsActive)
            //        ScreenLock.RequestActive();
            //}
            //else
            //{
            //    if (ScreenLock.IsActive)
            //        ScreenLock.RequestRelease();
            //}

            DoCmd("SpeakCur()");
        }

        public void RunJs(string cmd)
        {
            webView.InjectJavascriptAsync(cmd).GetAwaiter();
        }

        // 将由JavaScript间接调用
        public void Log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }

        // 将由JavaScript间接调用
        public void InitRun(string cmd)
        {
            webView.InjectJavascriptAsync(GetJavaScriptString() + cmd).GetAwaiter();
        }

        // 将由JavaScript间接调用
        public void Speak(bool isEnd, string text)
        {
            // 正在朗读，先停止
            if (isSpeakingPart)
            {
                Cancel();
            }
            isSpeakingPart = true;

            cts = new CancellationTokenSource();
            CrossTextToSpeech.Current.Speak(text,
                                speakRate: (float)m_SpeakRate,
                                volume: (float)m_Volume,
                                pitch: (float)m_Pitch,
                                cancelToken: cts.Token).ContinueWith((t) =>
                                {
                                    isSpeakingPart = false;

                                    // 由于取消也会走此流程，所以需要退出
                                    if (isCancelling)
                                    {
                                        isCancelling = false;
                                        return;
                                    }

                                    if (isEnd)
                                    {
                                        Pause();
                                        return;
                                    }

                                    SpeakNext();
                                },
                                // 最后的TaskScheduler不能少，否则应用会崩溃，不同平台也有区别
                                Device.RuntimePlatform == Device.iOS ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current);
        }

        private void SpeakNext()
        {
            DoCmd("SpeakNext()");
        }

        private void Cancel()
        {
            if (cts?.IsCancellationRequested ?? false)
                return;

            if (cts == null)
                return;

            isCancelling = true;
            cts.Cancel();
        }
    }
}
