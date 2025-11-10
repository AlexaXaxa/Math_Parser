using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Math_Parser_1._0.View.UserControls.GraphControl;
using System.Windows;

namespace Math_Parser_1._0
{
    class DrawPointMode : IGraphMode
    {
        public void OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {

            figures.Add(CreatePoint( e,  g));
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
