using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.WebView.Abstractions.Enumerations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Reflection;
using System.IO;
using Xam.Plugin.WebView.Abstractions;

namespace XamWebSpeaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : ContentPage
    {
        public static WebPage Instance { get; private set; }
        public static FormsWebView WebView { get; private set; }

        private WebSpeaker webSpeaker;

        public void SetUrlOrText(string url, bool isText)
        {
            WebContent.ContentType = isText ? WebViewContentType.StringData : WebViewContentType.Internet;
            try
            {
                WebContent.Source = url;
            }
            catch(Exception ex)
            {
                WebContent.ContentType = WebViewContentType.StringData;
                WebContent.Source = ex.ToString();
            }
        }

        public WebPage()
        {
            Instance = this;

            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);

            webSpeaker = WebSpeaker.Instance;
            webSpeaker.SetWebView(WebContent);

            // 加载帮助文档
            WebContent.ContentType = WebViewContentType.LocalFile;
            WebContent.Source = "index.html";

            WebContent.OnContentLoaded += WebContent_OnContentLoaded;

            WebView = WebContent;
        }

        private void WebContent_OnContentLoaded(object sender, EventArgs e)
        {
            // 网页内容加载完后，注入脚本
            webSpeaker.StartPage();
        }

        private void Btn_Play_Clicked(object sender, EventArgs e)
        {
            webSpeaker.Play();
        }

        private void Btn_Pause_Clicked(object sender, EventArgs e)
        {
            webSpeaker.Pause();
        }
    }
}