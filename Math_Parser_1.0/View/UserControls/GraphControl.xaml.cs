using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

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
        public static double pointDiameter = 13;
        public static bool setPoint = true;
        public static GraphPoint point1 = null;
        public static GraphPoint point2;

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

        public static void CalculateOffset(Point down, Point up)
        {

            offsetY = up.Y - down.Y;
            offsetX = up.X - down.X;

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

        static public GraphPoint CreatePoint(MouseButtonEventArgs e, Canvas g)
        {
            Ellipse newPoint = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 255);
            newPoint.Fill = mySolidColorBrush;
            newPoint.StrokeThickness = 1;
            newPoint.Stroke = Brushes.Black;
            newPoint.Width = pointDiameter;
            newPoint.Height = pointDiameter;
            Point position = e.GetPosition(g);

            offsetX = 0;
            offsetY = 0;

            return new GraphPoint("some point", "Point", newPoint, position);
        }
        static public void CreateSegment(MouseButtonEventArgs e, Canvas name)
        {
            
           

            if (setPoint)
            {
                point1 = CreatePoint(e, name);
                figures.Add(point1);
                setPoint = false;
            }
            else
            {
                point2 = CreatePoint(e, name);

                figures.Add(point2);
                var newLine = new Line();
                newLine.Stroke = System.Windows.Media.Brushes.Gray;
                newLine.X1 = point1.Position.X;
                newLine.X2 = point2.Position.X;
                newLine.Y1 = point1.Position.Y;
                newLine.Y2 = point2.Position.Y;


                newLine.HorizontalAlignment = HorizontalAlignment.Left;
                newLine.VerticalAlignment = VerticalAlignment.Center;
                newLine.StrokeThickness = 3;
                figures.Add(new GraphSegment("someSegment", "Segment", newLine, newLine));
                setPoint = true;
            }

        }

        private void DrawEveryFigure()
        {
          
            foreach (var f in figures)
            {
                f.UpdatePosition(offsetX, offsetY);
                Graph.Children.Add(f.Element);
            }
                
        }

        //public void RemoveFigure(string name)
        //{
        //    var fig = figures.FirstOrDefault(f => f.Name == name);
        //    if (fig != null)
        //    {
        //        Graph.Children.Remove(fig.Element); // убираем с Canvas
        //        figures.Remove(fig);                // убираем из списка
        //    }
        //}
        //public void RemoveAllPoints()
        //{
        //    var points = figures.Where(f => f.Type == "Point").ToList();
        //    foreach (var p in points)
        //    {
        //        Graph.Children.Remove(p.Element);
        //        figures.Remove(p);
        //    }
        //}

    }
}

