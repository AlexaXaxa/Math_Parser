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
        public double YwidthOffset;
        private double XheightOffset;
        private double distanceToX;
        private double distanceToY;
        public interface IGraphMode
        {
            public static Point downPoint;
            public static Point uppPoint;
            public static Cursor _customCursor = new Cursor("Assets/drag_cursor.cur");
            void OnMouseDown(Canvas name, MouseButtonEventArgs e);
            void OnMouseMove();
            void OnMouseUp(Canvas name, MouseButtonEventArgs e);


        }


        public GraphControl()
        {
            InitializeComponent();

            Graph.Loaded += (s, e) =>
            {
                XheightOffset = Graph.ActualHeight / 2;
                YwidthOffset = Graph.ActualWidth / 2;
                Graph.Children.Clear();
                DrawGrid(Brushes.Gray, 1);
                DrawAxes(Brushes.Black, 2);
            };

            Graph.SizeChanged += (s, e) =>
            {
                Graph.Children.Clear();
                DrawGrid(Brushes.Gray, 1);
                DrawAxes(Brushes.Black, 2);
            };

         

            Graph.MouseDown += Graph_MouseDown;
            Graph.MouseUp += Graph_MouseUp;
            Graph.Background = Brushes.Transparent; //не null

        }

        private void Graph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentMode.OnMouseUp(Graph, e);
            //RedrawAll();
            Graph.Children.Clear();

            DrawGrid(Brushes.Gray, 1);
            DrawAxes(Brushes.Black, 2);

        }
        //void RedrawAll()
        //{
        //    Graph.Children.Clear();

        //    // 1️⃣ Update axis offsets first
        //    // offsetX and offsetY are the current shifts (based on pan)
        //    XheightOffset = XheightOffset + offsetY;
        //    YwidthOffset = YwidthOffset + offsetX;

        //    // 2️⃣ Draw grid RELATIVE to updated offsets
        //    DrawGrid(Brushes.LightGray, 1);

        //    // 3️⃣ Draw axes using the same offsets
        //    DrawAxes(Brushes.Black, 2);
        //}

        void DrawAxes(Brush color, int thickness)
        {
            var Yaxis = new Line();
            Yaxis.Stroke = color;
            Yaxis.StrokeThickness = thickness;

            Yaxis.X1 = YwidthOffset + offsetX;
            Yaxis.X2 = YwidthOffset + offsetX;
            Yaxis.Y1 = 0;
            Yaxis.Y2 = Graph.ActualHeight;

            Graph.Children.Add(Yaxis);

            var Xaxis = new Line();
            Xaxis.Stroke = color;
            Xaxis.StrokeThickness = thickness;

            Xaxis.X1 = 0;
            Xaxis.X2 = Graph.ActualWidth;
            Xaxis.Y1 = XheightOffset + offsetY;
            Xaxis.Y2 = XheightOffset + offsetY;

            Graph.Children.Add(Xaxis);

            XheightOffset = XheightOffset + offsetY;
            YwidthOffset = YwidthOffset + offsetX;
        }
        void DrawGrid(Brush color, int thickness)
        {
            //берет старое значение оси
            double remainder = YwidthOffset % 50;
           
            for (double i = remainder; i<Graph.ActualWidth;i+=50 )
            {
                var Yaxis = new Line();
                Yaxis.Stroke = color;
                Yaxis.StrokeThickness = thickness;

                Yaxis.X1 = i + offsetX;
                Yaxis.X2 = i + offsetX;
                Yaxis.Y1 = 0;
                Yaxis.Y2 = Graph.ActualHeight;

                Graph.Children.Add(Yaxis);
            }
            remainder = XheightOffset % 50;
            for (double i = remainder; i < Graph.ActualHeight; i += 50)
            {
                var Xaxis = new Line();
                Xaxis.Stroke = color;
                Xaxis.StrokeThickness = thickness;

                Xaxis.X1 = 0;
                Xaxis.X2 = Graph.ActualWidth;
                Xaxis.Y1 = i + offsetY;
                Xaxis.Y2 = i + offsetY;

                Graph.Children.Add(Xaxis);
            }
       
        }

        private void Graph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentMode.OnMouseDown(Graph, e);
        }

        public static void CalculateOffset(Point one, Point two)
        {


            offsetY = two.Y - one.Y;
            offsetX = two.X - one.X;

        }
    }
}

