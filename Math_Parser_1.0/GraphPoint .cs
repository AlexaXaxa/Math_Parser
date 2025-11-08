using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Math_Parser_1._0
{
    public class GraphPoint: GraphFigure
    {
        public Point Position { get; set; }
        
        public GraphPoint(string name, string type, UIElement element, Point pos): base( name,  type)
        {
            Name = name;
            Type = type;
            Element = element;
            Position = pos;

        }

        public override void UpdatePosition(double offsetX, double offsetY)
        {
            var tempPosition = new Point(Position.X + offsetX, Position.Y + offsetY);
            Position = tempPosition;

            Canvas.SetLeft(Element, Position.X);
            Canvas.SetTop(Element, Position.Y);
        }

    }
}
