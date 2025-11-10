using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        public event Action<CustomTextBox> DeleteRequested;

        public event EventHandler EnterPressed;

        public CustomTextBox()
        {
            InitializeComponent();
        }



        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

            var btn = sender as Button;
            btn.ContextMenu.IsOpen = true;

        }

        private void Radera_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            DeleteRequested?.Invoke(this);
            txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
           if(string.IsNullOrEmpty(txtInput.Text))
           {
                tblPlaceholder.Visibility = Visibility.Visible;
           }
           else
           {
                tblPlaceholder.Visibility = Visibility.Hidden;
           }
                
        }
    }
}
