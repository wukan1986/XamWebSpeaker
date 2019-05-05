using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Xam.Plugin.WebView.Droid;


namespace XamWebSpeaker.Droid
{
    [Activity(Label = "XamWebSpeaker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleInstance)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "text/plain")]
    [IntentFilter(new[] { Intent.ActionSendMultiple }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = "message/rfc882")]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataSchemes = new string[] { "http", "https" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FormsWebViewRenderer.Initialize();

            // 对控件进行修改，注意位置
            FormsWebViewRenderer.OnControlChanged += FormsWebViewRenderer_OnControlChanged;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            OnNewIntent(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            var txt = "";
            switch (intent.Action)
            {
                case Intent.ActionView:
                    txt = intent.Data.ToString();
                    break;
                case Intent.ActionSend:
                    txt = intent.GetStringExtra(Intent.ExtraText);
                    break;
                case Intent.ActionSendMultiple:
                    txt = intent.GetStringExtra(Intent.ExtraText);
                    break;
                case Intent.ActionMain:
                    break;
            }

            MainPage.Instance.SetInputText(txt);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if(keyCode == Keycode.Back)
            {
                if(WebPage.WebView.CanGoBack)
                {
                    WebPage.WebView.GoBack();
                }
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        private void FormsWebViewRenderer_OnControlChanged(object sender, Android.Webkit.WebView e)
        {
            // 在pc上调试webview
            // chrome://inspect/#devices
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
            WebSpeakerJs.Context = this;
            e.AddJavascriptInterface(WebSpeakerJs.Instance, WebSpeakerJs.INTERFACE);
        }
    }
}

