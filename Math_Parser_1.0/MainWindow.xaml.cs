using Math_Parser_1._0.View.UserControls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Math_Parser_1._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<CustomTextBox> Items { get; set; } = [];

        public List<Button> modeButtons = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            AddTextbox();

            modeButtons.Add(cursor_btn);
            modeButtons.Add(punkt_btn);
            modeButtons.Add(linje_btn);

            cursor_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        }



        public void AddTextbox()
        {
            CustomTextBox tb = new CustomTextBox();
            tb.EnterPressed += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(Items[Items.Count - 1].txtInput.Text))
                {
                    //do nothing
                }
                else
                {
                    AddTextbox();
                }
            }
            ; //textbox lyssnar på enterpressed event
            Items.Add(tb);
        }

        private void Menuwindowbtn_Click(object sender, RoutedEventArgs e)
        {
            if (menuControl.Visibility == Visibility.Visible)
                menuControl.Visibility = Visibility.Collapsed;
            else
                menuControl.Visibility = Visibility.Visible;
        }

        private void cursor_btn_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;

            foreach (var btn in modeButtons)
            {
                if (btn == clicked)
                {
                    graph.currentMode = new MoveGridMode();
                    btn.BorderBrush = Brushes.Blue;    // активная кнопка                

                }
                else
                {
                    btn.ClearValue(Button.BorderBrushProperty);    // неактивные

                }

            }



        }

        private void punkt_btn_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            foreach (var btn in modeButtons)
            {
                if (btn == clicked)
                {
                    btn.BorderBrush = Brushes.Blue;    // активная кнопка
                }

                else
                {
                    btn.ClearValue(Button.BorderBrushProperty);    // неактивные
                }

            }

            graph.currentMode = new DrawPointMode();
        }
    }

}