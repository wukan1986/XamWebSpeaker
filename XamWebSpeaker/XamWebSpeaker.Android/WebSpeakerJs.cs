using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Webkit;
using Java.Interop;
using Plugin.TextToSpeech;
using Xam.Plugin.WebView.Abstractions;
using Xamarin.Forms;

namespace XamWebSpeaker.Droid
{
    public class WebSpeakerJs : Java.Lang.Object
    {
        public static WebSpeakerJs Instance { get; } = new WebSpeakerJs();

        protected WebSpeakerJs()
        {
        }

        public const string INTERFACE = "bell_pepper_info";

        [JavascriptInterface]
        [Export("Log")]
        public void Log(string text)
        {
            WebSpeaker.Instance.Log(text);
        }

        [JavascriptInterface]
        [Export("InitRun")]
        public void InitRun(string cmd)
        {
            WebSpeaker.Instance.InitRun(cmd);
        }

        [JavascriptInterface]
        [Export("Speak")]
        public void Speak(string text)
        {
            WebSpeaker.Instance.Speak(false, text);
        }

        [JavascriptInterface]
        [Export("SpeakEnd")]
        public void SpeakEnd(string text)
        {
            WebSpeaker.Instance.Speak(true, text);
        }
    }
}
