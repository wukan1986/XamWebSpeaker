using System;
using System.Collections.Generic;
using System.Linq;
using AVFoundation;
using Foundation;
using UIKit;
using Xam.Plugin.WebView.iOS;

namespace XamWebSpeaker.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            FormsWebViewRenderer.Initialize();

            FormsWebViewRenderer.OnControlChanged += FormsWebViewRenderer_OnControlChanged;

            AiForms.Renderers.iOS.SettingsViewInit.Init(); //need to write here

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // 后台播放音乐
            AVAudioSession avSession = AVAudioSession.SharedInstance();
            avSession.SetCategory(AVAudioSessionCategory.Playback);

            return base.FinishedLaunching(app, options);
        }

        private void FormsWebViewRenderer_OnControlChanged(object sender, WebKit.WKWebView e)
        {
            // Safari“开发”菜单项，启动app后，打开Safari 开发 模拟
            e.Configuration.UserContentController.AddScriptMessageHandler(WebSpeakerJs.Instance, WebSpeakerJs.Log);
            e.Configuration.UserContentController.AddScriptMessageHandler(WebSpeakerJs.Instance, WebSpeakerJs.InitRun);
            e.Configuration.UserContentController.AddScriptMessageHandler(WebSpeakerJs.Instance, WebSpeakerJs.Speak);
            e.Configuration.UserContentController.AddScriptMessageHandler(WebSpeakerJs.Instance, WebSpeakerJs.SpeakEnd);
        }
    }
}
