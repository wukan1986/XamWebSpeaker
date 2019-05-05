using Android.Content;
using Android.Webkit;
using Java.Interop;


namespace XamWebSpeaker.Droid
{
    public class WebSpeakerJs : Java.Lang.Object
    {
        public static WebSpeakerJs Instance { get; } = new WebSpeakerJs();


        public static Context Context;

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
            // Toast.MakeText(Context, "这是一个弹框！Speak", ToastLength.Short).Show();
            WebSpeaker.Instance.Speak(false, text);
        }

        [JavascriptInterface]
        [Export("SpeakEnd")]
        public void SpeakEnd(string text)
        {
            // Toast.MakeText(Context, "这是一个弹框！SpeakEnd", ToastLength.Short).Show();
            WebSpeaker.Instance.Speak(true, text);
        }
    }
}
