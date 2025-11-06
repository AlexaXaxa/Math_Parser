using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Math_Parser_1._0.View.UserControls.GraphControl;

namespace Math_Parser_1._0
{
    class DrawPointMode : IGraphMode
    {
        public void OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {
            Ellipse newPoint = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(255, 0, 0, 255);
            newPoint.Fill = mySolidColorBrush;

            newPoint.StrokeThickness = 1;
            newPoint.Stroke = Brushes.Black;

            newPoint.Width = 13;
            newPoint.Height = 13;

            Canvas.SetLeft(newPoint, e.GetPosition(g).X);
            Canvas.SetTop(newPoint, e.GetPosition(g).Y);

           
            figures.Add(new GraphFigure("some point", "Point", newPoint));
        }

        public void OnMouseMove(Canvas g, MouseEventArgs e)
        {
            // Implementation goes here
        }

        public void OnMouseUp(Canvas g, MouseButtonEventArgs e)
        {
            // Implementation goes here
        }
    }
}
