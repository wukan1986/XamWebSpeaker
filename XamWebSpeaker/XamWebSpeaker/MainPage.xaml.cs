using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamWebSpeaker
{
    public partial class MainPage : TabbedPage
    {
        public static MainPage Instance { get; private set; }

        public void SetInputText(string txt)
        {
            inputPage.SetText(txt);
            CurrentPage = inputPage;
        }

        public void SetUrl(string url, bool isText)
        {
            webPage.SetUrlOrText(url, isText);
            CurrentPage = webPage;
        }

        public MainPage()
        {
            Instance = this;

            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
        }
    }
}
