using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Math_Parser_1._0
{
    
    public abstract class GraphFigure
    {
        public string Name { get; set; }           // уникальное имя или идентификатор
        public UIElement Element { get; set; }     // сама фигура (Ellipse, Line и т.д.)
        public string Type { get; set; }           // "Point", "Line", "Ellipse" — для фильтров


        public GraphFigure(string name, string type)
        {
            Name = name;
            Type = type;

        }
        public abstract void UpdatePosition(double offsetX, double offsetY);

    }
}
