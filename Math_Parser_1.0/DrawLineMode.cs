using System.Windows.Controls;
using System.Windows.Input;
using static Math_Parser_1._0.View.UserControls.GraphControl;

namespace Math_Parser_1._0
{
    class DrawLineMode : IGraphMode
    {
        public string OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {
            // Implementation goes here
            return "0";
        }

        public void OnMouseMove(Canvas name, MouseEventArgs e)
        {
            // Implementation goes here
        }

        public void OnMouseUp(Canvas g, MouseButtonEventArgs e)
        {
            // Implementation goes here
        }
    }
}
