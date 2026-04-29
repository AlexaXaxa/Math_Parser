using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// InputTextbox, a part of IOPair logic element.
    /// </summary>
    public partial class Input : UserControl
    {
        public event Action<IOPair> DeleteRequested;

        public event Action<Input, string, InputMode> EnterPressed;

        //public bool IsBeingDeleted { get; set; } = false;
        public IOPair Owner { get; set; }
     
        public InputMode Mode { get; set; }
        public string Text
        {
            get { return txtInput.Text; }
            set { txtInput.Text = value; }
        }

        Brush defaultBrush;
        Brush textBrush = Brushes.AliceBlue;

        public Input()
        {
            InitializeComponent();
            Mode = InputMode.Math;
            defaultBrush = txtInput.BorderBrush;
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnPlus.IsEnabled = false;
                if (Mode == InputMode.Text)
                {
                    EnterPressed?.Invoke(this, txtInput.Text, InputMode.Text);
                }
                else
                    EnterPressed?.Invoke(this, txtInput.Text, InputMode.Math);
            }
        }

        private void Radera_Click(object sender, RoutedEventArgs e)
        {
            DeleteRequested?.Invoke(Owner); 
        }

        private void DuplicateOutput_Click(object sender, RoutedEventArgs e)
        {
            //dublicate output to the new input window
            EnterPressed?.Invoke(this, tblPlaceholder.Text, InputMode.Text);
        }

        void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            // к какому элементу «приклеено» меню
            button.ContextMenu.PlacementTarget = button;
            //где именно относительно кнопки
            button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            button.ContextMenu.IsOpen = true;

        }


        void Plus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            // к какому элементу «приклеено» меню
            button.ContextMenu.PlacementTarget = button;
            //где именно относительно кнопки
            button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            button.ContextMenu.IsOpen = true;
           
        }
        void Expression_Click (object sender, RoutedEventArgs e)
        {
            Mode = InputMode.Math;
            txtInput.BorderBrush = defaultBrush;
        }
        void Text_Click(object sender, RoutedEventArgs e)
        {
           
            Mode = InputMode.Text;
            txtInput.BorderBrush = textBrush;
        }
        void Image_Click(object sender, RoutedEventArgs e)
        {

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
