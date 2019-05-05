using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Xamarin.Essentials;

namespace XamWebSpeaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        void speak_rate_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            WebSpeaker.Instance.SpeakRate = e.NewValue;
        }

        void volume_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            WebSpeaker.Instance.Volume = e.NewValue;
        }

        void pitch_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            WebSpeaker.Instance.Pitch = e.NewValue;
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var x = ((Xamarin.Forms.Picker)sender).SelectedItem;
            System.Diagnostics.Debug.WriteLine(x);
        }

        void ScreenLock_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            WebSpeaker.Instance.IsScreenLock = e.Value;
            try
            {
                DeviceDisplay.KeepScreenOn = WebSpeaker.Instance.IsScreenLock;
            }
            catch
            {

            }
        }


        public SettingsPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);

            //var x = TextToSpeech.GetLocalesAsync().GetAwaiter().GetResult().ToList();
            //langs.ItemsSource = x;

            //screen_lock.On = WebSpeaker.Instance.IsScreenLock;
            //disable_image.IsEnabled = Device.RuntimePlatform == Device.Android;

            slider_speak_rate.Maximum = WebSpeaker.Instance.MaxSpeakRate;
            slider_speak_rate.Value = WebSpeaker.Instance.SpeakRate;
            slider_speak_rate.Minimum = WebSpeaker.Instance.MinSpeakRate;

            slider_volume.Maximum = 1;
            slider_volume.Value = WebSpeaker.Instance.Volume;
            slider_volume.Minimum = 0;

            slider_pitch.Maximum = 2;
            slider_pitch.Value = WebSpeaker.Instance.Pitch;
            slider_pitch.Minimum = 0;
        }

        private void btn_test_Clicked(object sender, EventArgs e)
        {
            WebSpeaker.Instance.Speak(true, "朗读测试，欢迎使用侃侃朗读，可选段的网页朗读神器", true);
        }

        private void btn_minus_Clicked(object sender, EventArgs e)
        {
            slider_speak_rate.Value -= 0.01;
        }

        private void btn_plus_Clicked(object sender, EventArgs e)
        {
            slider_speak_rate.Value += 0.01;
        }
    }
}