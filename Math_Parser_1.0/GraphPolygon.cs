using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows;

namespace Math_Parser_1._0
{
    class GraphPolygon : GraphFigure
    {
        PointCollection Collection;
        Polygon Poly;
        public GraphPolygon(string name, string type, Polygon element, PointCollection collection, Polygon poly) : base(name, type)
        {
            Name = name;
            Type = type;
            Collection = collection;
            //Element och Poly är REFERENSER till samma object. Andra Poly.
            Element = element;
            Poly = poly;
        }

        public override void UpdatePosition(double offsetX, double offsetY)
        {
            

            PointCollection myPointCollection = new PointCollection();
    

            foreach (var point in Collection)
            {
                Point p = new Point(point.X + offsetX, point.Y + offsetY);
                myPointCollection.Add(p);
            }

            Collection = myPointCollection;
            Poly.Points = Collection;
           
        }
    }
}
