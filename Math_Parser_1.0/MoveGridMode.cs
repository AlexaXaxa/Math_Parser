using System.Windows.Controls;
using System.Windows.Input;
using static Math_Parser_1._0.View.UserControls.GraphControl;



namespace Math_Parser_1._0
{

    class MoveGridMode : IGraphMode
    {


        public void OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {

            g.Cursor = IGraphMode._customCursor;

            IGraphMode.downPoint = e.GetPosition(g);
            double x = IGraphMode.downPoint.X;
            double y = IGraphMode.downPoint.Y;

        }

        public void OnMouseMove(Canvas g, MouseEventArgs e)
        {
        
        }

        public void OnMouseUp(Canvas g, MouseButtonEventArgs e)
        {
            g.Cursor = Cursors.Arrow;

            IGraphMode.uppPoint = e.GetPosition(g);
            double x = IGraphMode.downPoint.X;
            double y = IGraphMode.downPoint.Y;

            Math_Parser_1._0.View.UserControls.GraphControl.CalculateOffset(IGraphMode.downPoint, IGraphMode.uppPoint);
            
     
            
        }
    }
}
