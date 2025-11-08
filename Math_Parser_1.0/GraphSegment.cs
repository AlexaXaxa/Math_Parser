using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Shapes;
namespace Math_Parser_1._0
{
    internal class GraphSegment : GraphFigure
    {
        GraphPoint P1 { get; set; }
        GraphPoint P2 { get; set; }
       
        public GraphSegment(string name, string type, UIElement element, GraphPoint point1, GraphPoint point2) : base(name, type)
        {
            Name = name;
            Type = type;
            Element = element;
            P1 = point1;
            P2 = point2;
        }

        public override void UpdatePosition(double offsetX, double offsetY)
        {
            //Element.X1 = P1.X;
            //Element.X2 = P2.X;
            //Element.Y1 = P1.Y;
            //Element.Y2 = P2.Y;
        }
    }
}
