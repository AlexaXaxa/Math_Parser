using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml 
    /// </summary>
    public partial class GraphControl : UserControl
    {
        public IGraphMode currentMode;
        public static double offsetX = 0;
        public static double offsetY = 0;
        public static double YAxiswidthOffset;
        private static double XAxisheightOffset;
        public static List<GraphFigure> figures = new List<GraphFigure>();

        public interface IGraphMode
        {
            public static Point downPoint;
            public static Point uppPoint;
            public static Cursor _customCursor = new Cursor("Assets/drag_cursor.cur");
            void OnMouseDown(Canvas name, MouseButtonEventArgs e);
            void OnMouseMove(Canvas name, MouseEventArgs e);
            void OnMouseUp(Canvas name, MouseButtonEventArgs e);


        }
        //идея с абстрактным классом. все разные фигуры аппдейтят свою позицию сами. перерисовка идет в ме
        //тоде redrawallfigures где итерация через лист типа GraphFigure
        public class GraphFigure
        {
            public string Name { get; set; }           // уникальное имя или идентификатор
            public UIElement Element { get; set; }     // сама фигура (Ellipse, Line и т.д.)
            public string Type { get; set; }           // "Point", "Line", "Ellipse" — для фильтров
        

            public GraphFigure(string name, string type, UIElement element)
            {
                Name = name;
                Type = type;
                Element = element;
              
            }

            public void UpdatePosition(double offsetX, double offsetY, double zoomFactor = 1)
            {
                
            }
        }



        public GraphControl()
        {
            InitializeComponent();

            Graph.Loaded += (s, e) =>
            {
                XAxisheightOffset = Graph.ActualHeight / 2;
                YAxiswidthOffset = Graph.ActualWidth / 2;
                Graph.Children.Clear();
                DrawGrid(Brushes.Gray, 1);
                DrawAxes(Brushes.Black, 2);
            };

            Graph.SizeChanged += (s, e) =>
            {
                Graph.Children.Clear();
                DrawAxes(Brushes.Black, 2);
                DrawGrid(Brushes.Gray, 1);
                DrawEveryFigure();
            };

         

            Graph.MouseDown += Graph_MouseDown;
            Graph.MouseUp += Graph_MouseUp;
            Graph.Background = Brushes.Transparent; //не null

        }


       

        private void Graph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentMode.OnMouseUp(Graph, e);
          
            Graph.Children.Clear();
            DrawAxes(Brushes.Black, 2);
            DrawGrid(Brushes.Gray, 1);
            DrawEveryFigure();

        }
        private void Graph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentMode.OnMouseDown(Graph, e);
        }

        public static void CalculateOffset(Point one, Point two)
        {


            offsetY = two.Y - one.Y;
            offsetX = two.X - one.X;

            XAxisheightOffset = XAxisheightOffset + offsetY;
            YAxiswidthOffset = YAxiswidthOffset + offsetX;

        }

        void DrawAxes(Brush color, int thickness)
        {
            var Yaxis = new Line();
            Yaxis.Stroke = color;
            Yaxis.StrokeThickness = thickness;

            Yaxis.X1 = YAxiswidthOffset;
            Yaxis.X2 = YAxiswidthOffset;
            Yaxis.Y1 = 0;
            Yaxis.Y2 = Graph.ActualHeight;

            Graph.Children.Add(Yaxis);

            var Xaxis = new Line();
            Xaxis.Stroke = color;
            Xaxis.StrokeThickness = thickness;

            Xaxis.X1 = 0;
            Xaxis.X2 = Graph.ActualWidth;
            Xaxis.Y1 = XAxisheightOffset;
            Xaxis.Y2 = XAxisheightOffset;

            Graph.Children.Add(Xaxis);

           
        }
        void DrawGrid(Brush color, int thickness)
        {
            //YGrid
            double remainder = YAxiswidthOffset % 50;
            for (double i = remainder; i<Graph.ActualWidth; i+=50 )
            {
                var Yaxis = new Line();
                Yaxis.Stroke = color;
                Yaxis.StrokeThickness = thickness;

                Yaxis.X1 = i;
                Yaxis.X2 = i;
                Yaxis.Y1 = 0;
                Yaxis.Y2 = Graph.ActualHeight;

 
                if (Yaxis.X1 != YAxiswidthOffset || Yaxis.X2 != YAxiswidthOffset || Yaxis.Y1 != 0 || Yaxis.Y2 != Graph.ActualHeight)
                    Graph.Children.Add(Yaxis);
            }
            //XGrid
            remainder = XAxisheightOffset % 50;
            for (double i = remainder; i < Graph.ActualHeight; i += 50)
            {
                var Xaxis = new Line();
                Xaxis.Stroke = color;
                Xaxis.StrokeThickness = thickness;

                Xaxis.X1 = 0;
                Xaxis.X2 = Graph.ActualWidth;
                Xaxis.Y1 = i;
                Xaxis.Y2 = i;


                if (Xaxis.X1 != 0 || Xaxis.X2 != Graph.ActualWidth || Xaxis.Y1 != XAxisheightOffset || Xaxis.Y2 != XAxisheightOffset)
                    Graph.Children.Add(Xaxis);
            
            }

        }
        private void DrawEveryFigure()
        {
            foreach (var f in figures)
                Graph.Children.Add(f.Element);
        }
        public void RemoveFigure(string name)
        {
            var fig = figures.FirstOrDefault(f => f.Name == name);
            if (fig != null)
            {
                Graph.Children.Remove(fig.Element); // убираем с Canvas
                figures.Remove(fig);                // убираем из списка
            }
        }
        public void RemoveAllPoints()
        {
            var points = figures.Where(f => f.Type == "Point").ToList();
            foreach (var p in points)
            {
                Graph.Children.Remove(p.Element);
                figures.Remove(p);
            }
        }
        


    }
}

