using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomMenu.xaml
    /// </summary>
    public partial class CustomMenu : UserControl
    {

        public Action NextClicked;
        public Action PreviousClicked;
        public AudioService Audio { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer scroll_timer = new();
        private double _offset = 0;
        public CustomMenu() 
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            scroll_timer.Interval = TimeSpan.FromSeconds(0.2);
            scroll_timer.Tick += ScrollTimer_Tick;
            scroll_timer.Start();
        }
        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            //полная ширина контента 
            //- TrackScroll.ViewportWidth ширина видимой области
            double max = TrackScroll.ExtentWidth;
            double half = max / 2;


            if (TrackScroll.ExtentWidth <= TrackScroll.ViewportWidth)
                return;

            _offset += 1;

            if (_offset >= half)
                _offset = 0;

            TrackScroll.ScrollToHorizontalOffset(_offset);
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
        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Audio files (*.mp3)|*.mp3|All filles (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;

                var file = TagLib.File.Create(path);

                string title = file.Tag.Title;
                string artist = file.Tag.FirstPerformer;

                if (string.IsNullOrEmpty(title))
                    title = System.IO.Path.GetFileNameWithoutExtension(path);

                if (string.IsNullOrEmpty(artist))
                    artist = "Unknown";

                Audio.Load(path);

                SetInfo(title, artist);

                SetCover
                (
                    new BitmapImage
                    (
                        new Uri
                        (
                            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio/Cover/default.jpg")
                        )
                    )
                );

               
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {

            NextClicked?.Invoke();
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {

            PreviousClicked?.Invoke();
        }
        public void SetCover(ImageSource image)
        {
            CoverImage.Source = image;
        }
        public void SetInfo(string title, string author)
        {
            string line = $"{title} by {author}    |    ";
            TrackInfoText.Text = line + line;
            
        }
    }
}
