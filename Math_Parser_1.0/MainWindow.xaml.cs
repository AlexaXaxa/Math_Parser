using Math_Parser_1._0.View.UserControls;
using parsertut;
using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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


Argument x = new Argument("x = 2");
Constant a = new Constant("a = sin(10)");
Function f = new Function("f(t) = t^2");
Expression e = new Expression("2*x + a - f(10)", x, a, f);
double v = e.calculate();


3x^2 + 4*x - 5





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



y = 3x
x = y / 3
3x - y = 0


left - right = 0

implicit equation (




Сначала элементарное x= y=
потом cas, то есть алгебра, Solve,  List<Arrow> solutions = system.Solve(x, y);
 
---------------
Сделать x = expression
удаление текстбокса ведет к удалению информации о фигурах 
---------------
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
            KeyUp += keyUp;
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
                //graph.figuresInfo.Remove()
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
            // пробежаться по вариантам типа x= y=
            // привести ответ их к типу который может понять граф и отправить ему данные на перерисовку
            // вывести ответ юзеру в понятном виде


            /*
             
             class Command { }
class XLineCommand : Command { public double X; }
class YLineCommand : Command { public double Y; }
class FunctionCommand : Command { public string Expression; }

MainWindow не знает как рисовать. Он только решает "что рисовать".

GraphControl не знает, кто ввёл команду. Он просто рисует список команд.
             */

            Node answerNode;
            double answerNumber;

            try
            {

                //input: x = expression : x = 5+9
                if (input.Text.StartsWith("x="))
                {
                    string expr = input.Text[2..];

                    answerNode = Parser.Parse(expr);
                    answerNumber = answerNode.Eval(ctx);

                    //занести линию в память
                    graph.figuresInfo.Add(new XLineInfo(answerNumber));

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
                        graph.figuresInfo.Add(new FunctionInfo(ctx, expr));
                    }
                    //Yline
                    else
                    {
                        answerNode = Parser.Parse(expr);
                        answerNumber = answerNode.Eval(ctx);

                        //занести линию в память
                        graph.figuresInfo.Add(new YLineInfo(answerNumber));

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
                
            }
           
        }
  

        //____________________________BUTTONS_______________________________________________________________//
        private void Menuwindowbtn_Click(object sender, RoutedEventArgs e)
        {
            if (menuControl.Visibility == Visibility.Visible)
                menuControl.Visibility = Visibility.Collapsed;
            else
                menuControl.Visibility = Visibility.Visible;
        }
        private void Homebtn_Click(object sender, RoutedEventArgs e)
        {
            GraphControl.YAxiswidthOffset = graph.ActualWidth / 2;
            GraphControl.XAxisheightOffset = graph.ActualHeight / 2;
            graph.Redraw(graph.figuresInfo);
        }

        //--------------------------------------------------------------------//
        private void keyUp(object sender, KeyEventArgs e)
        {
            // Your handler code goes here
            if (e.Key == Key.H)
            {
                Help help = new();
                help.Show();
            }
        }
    }

}