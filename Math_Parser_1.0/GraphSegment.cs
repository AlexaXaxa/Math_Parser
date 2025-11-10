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
     
        public Point Position { get; set; }

        Line Line { get; set; }

       
        public GraphSegment(string name, string type, UIElement element, Line element2) : base(name, type)
        {
            Name = name;
            Type = type;
            //Element och Line är REFERENSER till samma skickade object.
            Element = element;
            Line = element2;

        }

        public override void UpdatePosition(double offsetX, double offsetY)
        {
            
            //Jag ändrar Line som pekar på samma object som Element.
            //När jag kallar elemet är den ändrat för att Line hade ändrar den.
            Line.X1 = Line.X1 + offsetX;
            Line.X2 = Line.X2 + offsetX;
            Line.Y1 = Line.Y1 + offsetY;
            Line.Y2 = Line.Y2 + offsetY;
      
        }
    }
}
