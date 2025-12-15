using Math_Parser_1._0.View.UserControls;
using parsertut;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Math_Parser_1._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<UIElement> Items { get; set; } = [];

        public List<Button> modeButtons = new List<Button>();

        public Context ctx = new Context();

        public MainWindow()
        {
            
            InitializeComponent();
           
            DataContext = this;
            AddTextbox();

            modeButtons.Add(cursor_btn);
            modeButtons.Add(punkt_btn);
            modeButtons.Add(linje_btn);
            modeButtons.Add(polygon_btn);

            cursor_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            graph.PointCreated += OnPointCreated;

        }

        private void OnPointCreated(string text)
        {
            var lastInput = Items.OfType<CustomTextBox>().LastOrDefault();
            if (lastInput == null)
                return;
            lastInput.txtInput.Text = text;
            AddTextbox();
        }

        public void AddTextbox()
        {
            CustomTextBox tb = new CustomTextBox();
            tb.DeleteRequested += Tb_DeleteRequested;
            tb.EnterPressed += Tb_EnterPressed;//textbox lyssnar på enterpressed event
            Items.Add(tb);
        }
        public void AddTextBoxOutput(string output)
        {
            OutputTextBox tb = new OutputTextBox(output);
            Items.Add(tb);
        }
        private void Tb_DeleteRequested(CustomTextBox tb)
        {

            if(Items.Count > 2)
            {
                Items.RemoveAt(Items.IndexOf(tb) + 1);
                Items.Remove(tb);
                //if tb inehåller variabel assignment radera den ur context

           
            }


            //GraphControl.figures.RemoveAll(f => f.Name == tb.Content);

        }
        private void Tb_EnterPressed(CustomTextBox sender, string text)
        {
            var current = (CustomTextBox)sender;

            if (string.IsNullOrWhiteSpace(text)) //если в боксе куда мы нажали энтер пусто
                return;

            //если не пусто там где мы нажали
            bool isLast = Items.OfType<CustomTextBox>().Last() == current;
            if (isLast) //если не пусто в последнем
            {
                // Это последний textbox → нужно создать output
                AddTextBoxOutput(Parser.Parse(text).Eval(ctx).ToString());

                // а потом добавить новый textbox
                AddTextbox();
            }
            else //если не путо в каком то другом
            {
                // Это НЕ последний textbox → просто обновить output под ним
                int index = Items.IndexOf(current);
                // Output всегда стоит сразу после CustomTextBox
                OutputTextBox outBox = (OutputTextBox)Items[index + 1];


                //обработать текст
                var trimed = text.Trim();
                if (trimed.StartsWith("x="))
                {
                    outBox.SetText(Parser.Parse(trimed).Eval(ctx).ToString());
                    //DrawVerticalLine
                }
                else //если текст поменялся
                {
                    outBox.SetText(Parser.Parse(text).Eval(ctx).ToString());
                }

            }



            
            
        }

//____________________________MENU_BUTTON_______________________________________________________________//
        private void Menuwindowbtn_Click(object sender, RoutedEventArgs e)
        {
            if (menuControl.Visibility == Visibility.Visible)
                menuControl.Visibility = Visibility.Collapsed;
            else
                menuControl.Visibility = Visibility.Visible;
        }

//____________________________BUTTONS________________________________________________________________//
        private void SetMode(Button clicked, IGraphMode mode)
        {
            foreach (var btn in modeButtons)
            {
                if (btn == clicked)
                {
                    graph.currentMode = mode;
                    btn.BorderBrush = Brushes.Blue;    // активная кнопка                

                }
                else
                {
                    btn.ClearValue(Button.BorderBrushProperty);    // неактивные

                }

            }
        }
        private void cursor_btn_Click(object sender, RoutedEventArgs e)
        {
            SetMode(sender as Button, new MoveGridMode());
        }
        private void punkt_btn_Click(object sender, RoutedEventArgs e)
        {
            SetMode(sender as Button, new DrawPointMode());
        }
        private void linje_btn_Click(object sender, RoutedEventArgs e)
        {
            SetMode(sender as Button, new DrawSegmentMode());
        }
        private void polygon_btn_Click(object sender, RoutedEventArgs e)
        {
            SetMode(sender as Button, new DrawPolygonMode());
        }
    }

}