using System;

using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Xamarin.Essentials;
using System.IO;
using System.Windows.Input;

namespace XamWebSpeaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputPage : ContentPage
    {
        public void SetText(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
                return;

            entry_input.Text = txt;
        }

        private bool tempfile_switch = true;
        void Go_Clicked(bool is_text)
        {
            var txt = entry_input.Text.Trim();
            if (is_text)
            {
                // 改成网页格式
                txt = txt.Replace("\n", "</p><p>");
                txt = "<html><p>" + txt + "</p></html>";
                var cacheDir = FileSystem.CacheDirectory;

                // 如果被打开了，将会锁定，所以切换一下文件名
                var templateFileNmae = Path.Combine(cacheDir, tempfile_switch ? "temp1.html" : "temp2.html");
                tempfile_switch = !tempfile_switch;

                File.WriteAllText(templateFileNmae, txt, Encoding.UTF8);
            }
            else
            {
                if (!txt.StartsWith("http"))
                {
                    txt = "http://" + txt;
                }
            }

            if (string.IsNullOrWhiteSpace(txt))
                return;

            MainPage.Instance.SetUrl(txt, is_text);
        }

        void Paste_Clicked()
        {
            if (Clipboard.HasText)
            {
                entry_input.Text = Clipboard.GetTextAsync().GetAwaiter().GetResult();
            }
        }

        public InputPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);

            PasteCommand = new Command(() => Paste_Clicked());
            GoTxtCommand = new Command(() => Go_Clicked(true));
            GoUrlCommand = new Command(() => Go_Clicked(false));

            BindingContext = this;
        }

        public ICommand PasteCommand { private set; get; }
        public ICommand GoTxtCommand { private set; get; }
        public ICommand GoUrlCommand { private set; get; }
    }
}