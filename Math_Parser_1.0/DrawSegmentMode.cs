using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using static Math_Parser_1._0.View.UserControls.GraphControl;

namespace Math_Parser_1._0
{
    internal class DrawSegmentMode : IGraphMode
    {
        bool setPoint = true;
        GraphPoint point1;
        GraphPoint point2;
        public void OnMouseDown(Canvas name, MouseButtonEventArgs e)
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

                //newLine.HorizontalAlignment = HorizontalAlignment.Left;
                //newLine.VerticalAlignment = VerticalAlignment.Center;
                newLine.StrokeThickness = 3;
                figures.Add(new GraphSegment("someSegment", "Segment", newLine, point1, point2));
                //myGrid.Children.Add(myLine);
            }
            



        }

        public void OnMouseMove(Canvas name, MouseEventArgs e)
        {
            
        }

        public void OnMouseUp(Canvas name, MouseButtonEventArgs e)
        {
          

            //ставится точка
            //ставися вторая точка
            //если количество точек кратно 2 то надо нарисовать отезок между ними.
        }
    }
}
