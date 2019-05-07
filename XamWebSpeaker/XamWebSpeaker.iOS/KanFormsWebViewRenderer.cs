using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using WebKit;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Abstractions.Enumerations;
using Xam.Plugin.WebView.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;


[assembly: Xamarin.Forms.ExportRenderer(typeof(FormsWebView), typeof(KanFormsWebViewRenderer))]
namespace Xam.Plugin.WebView.iOS
{
    public class KanFormsWebViewRenderer : FormsWebViewRenderer
    {
        [Export("webView:createWebViewWithConfiguration:forNavigationAction:windowFeatures:")]
        public virtual WKWebView CreateWebView(WKWebView webView, WKWebViewConfiguration configuration, WKNavigationAction navigationAction, WKWindowFeatures windowFeatures)
        {
            // First option - but this is not suitable for me
            //UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
            //return null;
            // end first option

            // Second option - but it cause some other problem to me
            var request = new NSMutableUrlRequest(navigationAction.Request.Url);

            Control.LoadRequest(request);
            return null;
            // end Second option
        }
    }
}
