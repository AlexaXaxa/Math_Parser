using Math_Parser_1._0;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        #region var declaration
     
       // public static double pointDiameter = 13;
       // public static bool setPoint = true;

        private Point downPoint;
        private Point uppPoint;
        public static Cursor drag_cursor = new Cursor("Assets/drag_cursor.cur");
        public static double offsetX = 0;
        public static double offsetY = 0;
        public static double YAxiswidthOffset { get; set; }
        public static double XAxisheightOffset { get; set; }

        public Dictionary<Input, GraphFigureInfo> figuresInfo { get; set; } = new(); //?? я не знаю использую ли я его правильно


        Line Yaxis;
        Line Xaxis;

        int pxPerdivition = 50;
        #endregion
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
                //DrawEveryFigure();
            };

            Graph.MouseDown += MouseDown;
            Graph.MouseUp += MouseUp;
            Graph.Background = Brushes.Transparent; //не null 
        }
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Graph.Cursor = drag_cursor;

            downPoint = e.GetPosition(Graph);
        }
        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Graph.Cursor = Cursors.Arrow;

            uppPoint = e.GetPosition(Graph);
            
            CalculateOffset(downPoint, uppPoint);

            Redraw(figuresInfo);
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
            Yaxis = new Line();
            Yaxis.Stroke = color;
            Yaxis.StrokeThickness = thickness;

            Yaxis.X1 = YAxiswidthOffset;
            Yaxis.X2 = YAxiswidthOffset;
            Yaxis.Y1 = 0;
            Yaxis.Y2 = Graph.ActualHeight;

            Graph.Children.Add(Yaxis);

            Xaxis = new Line();
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
            double remainder = YAxiswidthOffset % pxPerdivition;
            for (double i = remainder; i < Graph.ActualWidth; i += pxPerdivition)
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
            remainder = XAxisheightOffset % pxPerdivition;
            for (double i = remainder; i < Graph.ActualHeight; i += pxPerdivition)
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
        public void Redraw(Dictionary<Input, GraphFigureInfo> dict)
        {
            Graph.Children.Clear();
            DrawAxes(Brushes.Black, 2);
            DrawGrid(Brushes.Gray, 1);
            DrawEveryFigure(dict);            
        }

        private void DrawEveryFigure(Dictionary<Input, GraphFigureInfo> dict)
        {
            foreach (var item in dict.Values)
            {
                if (item is XLineInfo x)
                {
                    Line line = new();

                    line.X1 = XMathToScreen(x.X);
                    line.X2 = XMathToScreen(x.X);                    

                    line.Y1 = 0;
                    line.Y2 = Graph.ActualHeight;
                    line.Stroke = x.Color;
                    line.StrokeThickness = 3;
                    Graph.Children.Add(line);
                }
                else if (item is YLineInfo y)
                {
                    Line line = new();
                    line.SnapsToDevicePixels = true;

                    line.Y1 = (YMathToScreen(y.Y));
                    line.Y2 = (YMathToScreen(y.Y));

                    line.X1 = 0;
                    line.X2 = Graph.ActualWidth;
                    line.Stroke = y.Color;
                    line.StrokeThickness = 3;
                    Graph.Children.Add(line);

                }
                else if(item is FunctionInfo f)
                {
                    double stepScreen = 1;
                    double xScreen = 0;
                    double xEndScreen = Graph.ActualWidth;

                    while (xScreen < xEndScreen)
                    {
                        //что бы вычислить значение у
                        double xmath = XScreenToMath(xScreen);
                        //вычисляем значение у
                        double ymath = f.Function(xmath);
                        //экранные координаты у
                        double yScreen = YMathToScreen(ymath);

                        Ellipse ellipse = new();
                        ellipse.Width = 3;
                        ellipse.Height = 3;
                        ellipse.Fill = Brushes.Blue;
                        Canvas.SetLeft(ellipse, xScreen-3);
                        Canvas.SetTop(ellipse, yScreen+3);
                        Graph.Children.Add(ellipse);


                        xScreen += stepScreen;
                    }
                }

            }
        }

        double XMathToScreen(double math)
        {
            return Yaxis.X1 + pxPerdivition * math;
        }
        double YMathToScreen(double math)
        {
            return Xaxis.Y1 - pxPerdivition * math;
        }

        double XScreenToMath(double screen)
        {
            return (screen - YAxiswidthOffset) / pxPerdivition;
        }

        double YScreenToMath(double screen)
        {
            return (screen - XAxisheightOffset) / pxPerdivition;
        }

        //static public GraphPoint CreatePoint(MouseButtonEventArgs e, Canvas g)
        //{
        //    Ellipse newPoint = new Ellipse();
        //    SolidColorBrush mySolidColorBrush = new SolidColorBrush();

        //    mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 255);
        //    newPoint.Fill = mySolidColorBrush;
        //    newPoint.StrokeThickness = 1;
        //    newPoint.Stroke = Brushes.Black;
        //    newPoint.Width = pointDiameter;
        //    newPoint.Height = pointDiameter;
        //    Point position = e.GetPosition(g);

        //    offsetX = 0;
        //    offsetY = 0;

        //    return new GraphPoint("some point", "Point", newPoint, position);
        //}
        //static public void CreateSegment(MouseButtonEventArgs e, Canvas name)
        //{

        //    if (setPoint)
        //    {
        //        point1 = CreatePoint(e, name);
        //        figures.Add(point1);
        //        setPoint = false;
        //    }
        //    else
        //    {
        //        point2 = CreatePoint(e, name);

        //        figures.Add(point2);
        //        var newLine = new Line();
        //        newLine.Stroke = System.Windows.Media.Brushes.Gray;
        //        newLine.X1 = point1.Position.X;
        //        newLine.X2 = point2.Position.X;
        //        newLine.Y1 = point1.Position.Y;
        //        newLine.Y2 = point2.Position.Y;


        //        newLine.HorizontalAlignment = HorizontalAlignment.Left;
        //        newLine.VerticalAlignment = VerticalAlignment.Center;
        //        newLine.StrokeThickness = 3;
        //        figures.Add(new GraphSegment("someSegment", "Segment", newLine, newLine));
        //        setPoint = true;
        //    }

        //}

        //private void DrawEveryFigure()
        //{
        //  //
        //    foreach (var f in figures)
        //    {
        //        f.UpdatePosition(offsetX, offsetY);
        //        Graph.Children.Add(f.Element);
        //    }

        //}

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

