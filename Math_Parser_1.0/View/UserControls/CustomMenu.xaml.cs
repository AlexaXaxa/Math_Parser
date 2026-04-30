using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomMenu.xaml
    /// </summary>
    public partial class CustomMenu : UserControl
    {
        public AudioService Audio { get; set; }
        DispatcherTimer timer = new DispatcherTimer();

        public CustomMenu() 
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Audio == null) return;

            double duration = Audio.TotalSeconds;
            TimeSpan position = Audio.GetPos();

            if (duration == 0) return;

            double percent = (position.TotalSeconds / duration) * 100;

            Slider.Value = percent;
        }
        private void Helpbtn(object sender, System.Windows.RoutedEventArgs e)
        {
            //shortcut?

            Help help = new();
            help.Show();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Audio.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Audio.Pause();
        }
        private void Rewind15_Click(object sender, RoutedEventArgs e)
        {
            Audio.Rewind15();
        }
        private void Forward15_Click(object sender, RoutedEventArgs e)
        {
            Audio.Forward15();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Audio.HasTimeSpan() == false) 
                return;


            double totalSec = Audio.TotalSeconds;

            double newsec = (Slider.Value / 100) * totalSec;

            Audio.SetPos(newsec);

        }

        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            string email = "workemail@outlook.com";
            string subject = "Feedback";
            string body = "Your message here...";

            string mailto = $"mailto:{email}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

            try
            {
                Process.Start(new ProcessStartInfo(mailto)
                {
                    UseShellExecute = true
                });
            }
            catch 
            {
                Clipboard.SetText(email);
                MessageBox.Show(
                    "We couldn't open your email app.\nEmail copied to clipboard.",
                    "Feedback",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }                

        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string email = "workemail@outlook.com";

            Clipboard.SetText(email);
            MessageBox.Show(
                "Email copied to clipboard.",
                "Feedback",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }
    }
}
