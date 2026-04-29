using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomMenu.xaml
    /// </summary>
    public partial class CustomMenu : UserControl
    {
        public AudioService Audio { get; set; }
        private bool _isDragging = false;
        public CustomMenu()
        {
            InitializeComponent();
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
        
    }
}
