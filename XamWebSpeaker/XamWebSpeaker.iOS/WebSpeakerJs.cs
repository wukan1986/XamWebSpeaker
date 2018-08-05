using Foundation;
using System;
using WebKit;

namespace XamWebSpeaker.iOS
{
    public partial class WebSpeakerJs : NSObject, IWKScriptMessageHandler
    {
        public const string Log = "Log";
        public const string InitRun = "InitRun";
        public const string Speak = "Speak";
        public const string SpeakEnd = "SpeakEnd";

        public static WebSpeakerJs Instance { get; } = new WebSpeakerJs();

        protected WebSpeakerJs()
        {
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            switch(message.Name)
            {
                case Log:
                    _Log(message.Body.ToString());
                    break;
                case InitRun:
                    _InitRun(message.Body.ToString());
                    break;
                case Speak:
                    _Speak(message.Body.ToString());
                    break;
                case SpeakEnd:
                    _SpeakEnd(message.Body.ToString());
                    break;
            }
        }

        private void _Log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        
        private void _InitRun(string cmd)
        {
            WebSpeaker.Instance.InitRun(cmd);
        }
        
        private void _Speak(string text)
        {
            WebSpeaker.Instance.Speak(false, text);
        }
        
        private void _SpeakEnd(string text)
        {
            WebSpeaker.Instance.Speak(true, text);
        }
    }
}
