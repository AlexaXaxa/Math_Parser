using Math_Parser_1._0.View.UserControls;

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


using org.mariuszgromada.math.mxparser;
using Expression = org.mariuszgromada.math.mxparser.Expression;

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

разноцветные линии функций
функции
*/

namespace Math_Parser_1._0
{
    public partial class MainWindow : Window
    {    
        //i need rows to operate text in textboxes. Set, get text.
        public ObservableCollection<IOPair> Rows { get; set; } = [];

        
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public Dictionary<string, Argument> arguments = [];
        public Dictionary<string, Function> functions = [];

        public MainWindow()
        {
            License.iConfirmNonCommercialUse("Aleksa");
            Initialize();
            KeyUp += MainWindowKey;
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

        //может только назначать переменные
        //может сохранять переменные
        //может выводить переменные по имени
        //может решать выражения без переменных
        //может решать выражения с переменными
        //может хранить зависимые переменные
        //может хранить функции типа f(x)=x 
        //может вызывать функиции f(2)

        //переменная в функции
        void UppdateState(Input input)
        {
            string str = input.Text;


            if (!str.Contains("="))
            {
                //variable output
                if (arguments.ContainsKey(str))
                {
                    input.Owner.Output.Text =
                        arguments[str].getArgumentValue().ToString();
                    return;
                }
                
                //expression
                Expression e = new(str, arguments.Values.Cast<PrimitiveElement>()
        .Concat(functions.Values)
        .ToArray());
                double answer = e.calculate();
                input.Owner.Output.Text = answer.ToString();
                return;
            }

            //new assignmen
            else
            {
                //function
                if (str.Contains('('))
                {

                    Function f = new(str);  

                    string fname = f.getFunctionName();

                    functions[fname] = f;

                    input.Owner.Output.Text = "function defined";

                    graph.Redraw(graph.figuresInfo);
                    return;
                }
                //variable
                Argument newArg = new Argument(str, arguments.Values.ToArray());
                string name = newArg.getArgumentName();

                arguments[name] = newArg; //old object rewrites, new creates.

                input.Owner.Output.Text = name + " = " + newArg.getArgumentValue();

                graph.Redraw(graph.figuresInfo);
                    
            }
            
            


            //try
            //{

            //    if (input.Text.StartsWith("x="))
            //    {
            //        string expr = input.Text[2..];

            //        org.mariuszgromada.math.mxparser.Expression ee = new org.mariuszgromada.math.mxparser.Expression(expr);

            //        Argument x = new Argument(ee.ToString());




            //        //занести линию в память
            //        //graph.figuresInfo.Add(new XLineInfo(4));

            //        //нарисовать из памяти все линии
            //        graph.Redraw(graph.figuresInfo);

            //        //вывести ответ юзеру
            //        //input.Owner.Output.Text = "x = " + answerNumber.ToString();


            //    }

            //    //input: y = expresion: y = 2+6 OR function y=x
            //    else if (input.Text.StartsWith("y="))
            //    {
            //        //after y=
            //        string expr = input.Text[2..];

            //        //function
            //        if (input.Text.Contains("x"))
            //        {
            //            //занести в память
            //            //graph.figuresInfo.Add(new FunctionInfo(ctx, expr));
            //        }
            //        //Yline
            //        else
            //        {
            //            //answerNode = Parser.Parse(expr);
            //            //answerNumber = answerNode.Eval(ctx);

            //            //занести линию в память
            //            //graph.figuresInfo.Add(new YLineInfo(answerNumber));

            //            //вывести ответ юзеру
            //            //input.Owner.Output.Text = "y = " + answerNumber.ToString();
            //        }      

            //    }
            //    //expression : 5+4
            //    else
            //    {
            //        //answerNode = Parser.Parse(input.Text);
            //        //обновить аутпут
            //        //input.Owner.Output.Text = answerNode.Eval(ctx).ToString();
            //    }

            //    //нарисовать из памяти все линии
            //    graph.Redraw(graph.figuresInfo);
            //}
            //catch (Exception e)
            //{
            //    input.Owner.Output.Text = $"{e.Message}";
            //    Console.WriteLine(e.Message);

            //}



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
            GraphControl.YAxiswidthOffset = graph.ActualWidth / 2;
            GraphControl.XAxisheightOffset = graph.ActualHeight / 2;
            graph.Redraw(graph.figuresInfo);
        }

        //____________________________Shortcuts___________________________________//
        private void MainWindowKey(object sender, KeyEventArgs e)
        {
            // Your handler code goes here
            if (e.Key == Key.H)
            {
                Help help = new();
                help.Show();
            }
            else if(e.Key == Key.M)
            {
                ToggleMenu();
            }
        }
        
    }

}