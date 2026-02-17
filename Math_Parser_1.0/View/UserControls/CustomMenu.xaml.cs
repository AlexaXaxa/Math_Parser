using System.Windows;
using System.Windows.Controls;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomMenu.xaml
    /// </summary>
    public partial class CustomMenu : UserControl
    {
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
    }
}
