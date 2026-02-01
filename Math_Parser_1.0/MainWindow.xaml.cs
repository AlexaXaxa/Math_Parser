using Math_Parser_1._0.View.UserControls;
using parsertut;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

/*

Единственное что мы можем делать когда прикасаемся к графу это двигать его
 
Есть один способ добавить объект на граф
С помощью текстинпута

Перерисовка:
    когда нажимается энтер с фокусом на текстбоксе
    когда двигается экран.


Инпут:
    Только вычисления выражений

                                    когда юзер пишет A = (1+2, 3) и нажимает на энтер то парсер выдает (3, 3) в аутпут
                                    код это обрабатывает в EnterPressed handler:
                                    Если текст который ввел юзер начинается с заглавной буквы то это тип точка
                                    Если Line то это тип линия 
                                    Надо назначать тип данных который там лежит. 


---------------
Сначала элементарное x= y=
потом cas, то есть алгебра, Solve,  List<Arrow> solutions = system.Solve(x, y);
 */

namespace Math_Parser_1._0
{
    public partial class MainWindow : Window
    {    
        //i need rows to operate text in textboxes. Set, get text.
        public ObservableCollection<IOPair> Rows { get; set; } = [];

        public Context ctx = new Context();
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            Initialize();
        }
        public void Initialize()
        {
            InitializeComponent();
            Title = "Geogebra Clone";
            Icon = new BitmapImage(new Uri("C:\\Users\\06aleden_edu.uppland\\Source\\Repos\\Math_Parser_1.0\\Math_Parser_1.0\\Assets\\geogebra_ico.ico"));
            WindowState = WindowState.Maximized;

            mediaPlayer.Open(new Uri("Audio/pocket_calculator.mp3", UriKind.Relative));
           // mediaPlayer.Play();

            InitialInputs();
        }
        private void InitialInputs()
        {
           
            AddIOPair(InputMode.Text);
            Rows[0].Input.Text = "Welcome to the GeoGebra Clone!";
            Rows[0].Input.txtInput.BorderBrush = Brushes.AliceBlue;
            Rows[0].Input.btnPlus.IsEnabled = false;

            AddIOPair(InputMode.Math);
            Rows[1].Input.Text = "3+6*2";
            Rows[1].Input.txtInput.Focus();

        }
        public void AddIOPair(InputMode mode)
        {
            var pair = new IOPair(); //create a new instanse of IOPair
            
            var grid = new Grid();
            pair.Container = grid;
            grid.RowDefinitions.Add(new RowDefinition()); //define rows
            grid.RowDefinitions.Add(new RowDefinition());

            Grid.SetRow(pair.Input, 0); //set row number on the pair
            Grid.SetRow(pair.Output, 1);

            grid.Children.Add(pair.Input); //add pair
            grid.Children.Add(pair.Output);

            InputFields.Children.Add(grid); //add grid
            Rows.Add(pair); //add pair in the list

            pair.Input.Owner = pair;
            pair.Input.Mode = mode;
            pair.Input.DeleteRequested += DeleteIOPair;
            pair.Input.EnterPressed += EnterPressed; //textbox lyssnar på enterpressed event
            pair.Input.LostFocus += Input_LostFocus;
            
        }

      

        public void DeleteIOPair(IOPair pair)
        {
            if (Rows.Count > 1)
            {
                //inputfields is visual
                InputFields.Children.Remove(pair.Container); //jag behöver att ha grid, som innehåller InputRow
                //rows is logical
                Rows.Remove(pair);
            }
            
        }
        //private void Tb_DeleteRequested(Row row)
        //{
        //    switch (row.Type)
        //    {
        //        case RowType.Variable:
        //            //ctx.RemoveVariable(row.VariableName);
        //            break;

        //        case RowType.Point:
        //            //graph.RemovePoint(row.Point.Value);
        //            break;

        //        case RowType.Expression:
        //            // ничего
        //            break;
        //    }


        //    //Row.Remove();


        //    //GraphControl.figures.RemoveAll(f => f.Name == tb.Content);

        //}

        private void EnterPressed(Input input, string text, InputMode mode)
        {
            if (!string.IsNullOrWhiteSpace(Rows.Last().Input.Text))
            {
                AddIOPair(InputMode.Math);
            }

            if (mode == InputMode.Text)
            {
                input.Owner.Output.Text = "";
            }
            else if(mode == InputMode.Math)
                UppdateState(input);
        }
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            Input input = (Input)sender;
            if (input.Mode == InputMode.Text)
            {
                input.Owner.Output.Text = "";
            }
            else if (input.Mode == InputMode.Math)
                UppdateState(sender as Input);
        }
        void UppdateState(Input input)
        {
            // вычеслить текстбокс - привести их к состоянию которое можно обработать перерисовщиком графа или к состоянию которое может прочитать юзер
            // перерисовать граф на основе содержания текстбоксов
            //вычеслить текстбокс 

            //если будет текс только то надо парсеру выдавать ""
            //если будет пусто то ""
            Node totalNode;

            try
            {
                totalNode = Parser.Parse(input.Text);
                input.Owner.Output.Text = totalNode.Eval(ctx).ToString();
                //перерисовать граф
                graph.Redraw();
            }
            catch (Exception e)
            {
                input.Owner.Output.Text = $"{e.Message}";
                Console.WriteLine(e.Message);
                
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
    }
}