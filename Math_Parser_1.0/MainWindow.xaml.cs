using Math_Parser_1._0.View.UserControls;
using parsertut;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


/*


Инпут:
    Только вычисления выражений 3+4
    x = выражение + видимый график

                                    когда юзер пишет A = (1+2, 3) и нажимает на энтер то парсер выдает (3, 3) в аутпут
                                    код это обрабатывает в EnterPressed handler:
                                    Если текст который ввел юзер начинается с заглавной буквы то это тип точка
                                    Если Line то это тип линия 
                                    Надо назначать тип данных который там лежит. 

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






Input
 ↓
Parser
 ↓
AST
 ↓
Analyzer (что это?)
 ↓
Figure (Line / Curve / Point)
 ↓
Renderer

уравнение (=)

точка (^[A-Z]\w*\s*\(.*\)$)

число (else)

потом решить как рисовать





син работает с радианами а не с градусами, изменить(?)


визуал
разноцветные линии функций
функции



-----ЦЕЛЬ----------

---------------
*/

namespace Math_Parser_1._0
{
    public partial class MainWindow : Window
    {    
        //i need rows to operate text in textboxes. Set, get text.
        public ObservableCollection<IOPair> Rows { get; set; } = [];

        public Context ctx = new Context();

        AudioService player = new();

        public ObservableCollection<Track> Tracks { get; set; } = new();
        int currentIndex = 0;

        public MainWindow()
        {
            
            Initialize();
            KeyUp += MainWindowKey;

            menuControl.NextClicked = Next;
            menuControl.PreviousClicked = Previous;
            
        }
        
        public void Initialize()
        {
            InitializeComponent();

            Title = "Geogebra Clone";
            Icon = new BitmapImage(new Uri("C:\\Users\\06aleden_edu.uppland\\Source\\Repos\\Math_Parser_1.0\\Math_Parser_1.0\\Assets\\geogebra_ico.ico"));
            WindowState = WindowState.Maximized;

            menuControl.Audio = player;
            FillTracks();
            StartPlayer();
            InitialInputs();
        }

        private void FillTracks()
        {
            
            Track pocket = new("Pocket calculator", "Kraftwerk", "Audio/pocket_calculator.mp3", "Audio/Cover/pocket_calculator.jpg");
            Track white_noise = new("White noise", "unknown", "Audio/white_noise.mp3", "Audio/Cover/white_noise.jpg", 0.1);
            Track piano = new("Piano", "unknown", "Audio/piano.mp3", "Audio/Cover/piano.jpg");
            Track birdsong = new("Birdsong", "unknown", "Audio/birdsong.mp3", "Audio/Cover/bird.webp");
            Track cat_purr = new("Cat purr", "unknown", "Audio/cat_purr.mp3", "Audio/Cover/cat_purr.webp");
            Track heater = new("Heater noise", "unknown", "Audio/heater_noise.mp3", "Audio/Cover/heater.jpg");

            

            Tracks.Add(pocket);
         
            Tracks.Add(white_noise);
            Tracks.Add(piano);
            Tracks.Add(birdsong);
            Tracks.Add(cat_purr);
            Tracks.Add(heater);

        }
        private void StartPlayer()
        {
            if (Tracks.Count == 0)
            {
                return;
            }
            currentIndex = 0;
            PlayCurrent();
        }
        private void PlayCurrent()
        {
            var track = Tracks[currentIndex];

            player.Load(track.Url);
            player.SetVolume(track.Volume);
            menuControl.SetCover(track.Cover);
            menuControl.SetInfo(track.Title, track.Artist);
           
        }
        private void Next()
        {
            currentIndex++;

            if (currentIndex >= Tracks.Count)
                currentIndex = 0; 
            
            PlayCurrent();
        }
        private void Previous()
        {
            if (Tracks.Count == 0)
                return;

            currentIndex--;

            if (currentIndex < 0)
                currentIndex = Tracks.Count - 1;

            PlayCurrent();
        }
        private void InitialInputs()
        {
           
            AddIOPair(InputMode.Text);
            Rows[0].Input.Text = "hi";
            Rows[0].Input.txtInput.BorderBrush = Brushes.AliceBlue;
            Rows[0].Input.btnPlus.IsEnabled = false;

            AddIOPair(InputMode.Math);
            Rows[1].Input.Text = "y=x";
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
                //pair.Input.IsBeingDeleted = true;

                pair.Input.Text = "";

                //graph.figuresInfo.Remove(pair.Input);
            

                //inputfields is visual
                InputFields.Children.Remove(pair.Container); //jag behöver att ha grid, som innehåller InputRow
                
                //rows is logical
                Rows.Remove(pair);

            }
            
        }


        private void EnterPressed(Input input, string text, InputMode mode)
        {
            //РЕШАЕМ ОТВЕТ
            //потом если мод текст то в ответе ""
            if (mode == InputMode.Text)
            {
                input.Owner.Output.Text = "";
            }  
            //если мод математика то обновитьстейт
            else if(mode == InputMode.Math)
                UppdateState(input);

            //ДОБАВЛЯЕМ ИНПУТ НИЖЕ
            //если в последнем инпуте что то есть то
            if (!string.IsNullOrWhiteSpace(Rows.Last().Input.Text))
            {
                AddIOPair(InputMode.Math);
            }
            //если ничего нет то ничего не делать
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

            //if (input.IsBeingDeleted)
            //{
            //    return;
            //}


            Node answerNode;
            double answerNumber;
            string str = input.Text;

            //удалить фигуру которая была в этом графике раньше
            graph.figuresInfo.Remove(input);


            try
            {
                if (input.Text.StartsWith("x="))
                {
                    string expr = input.Text[2..];


                    answerNode = Parser.Parse(expr);
                    answerNumber = answerNode.Eval(ctx);

                    //занести линию в память
                    graph.figuresInfo[input] = new XLineInfo(answerNumber);

                    //нарисовать из памяти все линии
                    graph.Redraw(graph.figuresInfo);

                    //вывести ответ юзеру
                    input.Owner.Output.Text = "x = " + answerNumber.ToString();
                    
                   
                }

                //input: y = expresion: y = 2+6 OR function y=x
                else if (input.Text.StartsWith("y="))
                {
                    //after y=
                    string expr = input.Text[2..];

                    //function
                    if (input.Text.Contains("x"))
                    {
                        //занести в память
                        
                        graph.figuresInfo[input] = new FunctionInfo(ctx, expr);
                    }
                    //Yline
                    else
                    {
                        answerNode = Parser.Parse(expr);
                        answerNumber = answerNode.Eval(ctx);

                        //занести линию в память
                        graph.figuresInfo[input] = new YLineInfo(answerNumber);
                        

                        //вывести ответ юзеру
                        input.Owner.Output.Text = "y = " + answerNumber.ToString();
                    }      

                }
                //expression : 5+4
                else
                {
                    answerNode = Parser.Parse(input.Text);
                    //обновить аутпут
                    input.Owner.Output.Text = answerNode.Eval(ctx).ToString();
                }

                //нарисовать из памяти все линии
                graph.Redraw(graph.figuresInfo);
            }
            catch (Exception e)
            {
                input.Owner.Output.Text = $"{e.Message}";
                Console.WriteLine(e.Message);
                graph.Redraw(graph.figuresInfo);
            }

        }


        //____________________________BUTTONS_______________________________________________________________//
        void ToggleMenu()
        {
            if (menuControl.Visibility == Visibility.Visible)
                menuControl.Visibility = Visibility.Collapsed;
            else
                menuControl.Visibility = Visibility.Visible;
        }
        private void Menuwindowbtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }
        private void Homebtn_Click(object sender, RoutedEventArgs e)
        {
            Home();
        }
        void Home()
        {
            GraphControl.YAxiswidthOffset = graph.ActualWidth / 2;
            GraphControl.XAxisheightOffset = graph.ActualHeight / 2;
            graph.Redraw(graph.figuresInfo);
        }
        //____________________________Shortcuts___________________________________//
        private void MainWindowKey(object sender, KeyEventArgs e)
        {
            // Your handler code goes here
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Help help = new();
                help.Show();
            }
            else if(e.Key == Key.M && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ToggleMenu();
            }
            else if(e.Key == Key.H && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Home();
            }
        }
        
    }

}