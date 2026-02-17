using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using parsertut;

namespace Math_Parser_1._0
{
    //public List<GraphFigureInfo> figuresInfo { get; set; } = new();
    //DrawEveryFigure(figuresInfo); 
    public abstract class GraphFigureInfo{    }




    //x = 5
    class XLineInfo : GraphFigureInfo
    {
        //вертикальная линия, нужно знать абсциссу
        public double X;
        public Brush Color = Brushes.Blue;
        
        public XLineInfo(double x)
        {
            X = x;
            
        }
        
    }
    //y = 5
    class YLineInfo : GraphFigureInfo
    {
        //горизонтальная линия, нужна ордината
        public double Y;
        public Brush Color = Brushes.Red;
        public YLineInfo(double y)
        {
            Y = y;


        }

    }

    //y=x or y=x+4+sin3
    class FunctionInfo : GraphFigureInfo
    {
        Context _ctx;
        //everything after y= like x+5
        string _expr;
        public FunctionInfo(Context ctx, string expr)
        {
            _ctx = ctx;
            _expr = expr;
        }

        public double Function(double xmath)
        {
            //занести переменную x в контекст
            _ctx.SetVariable("x", xmath);
            // сделать вычисления через парсер (значит нам нужно хранить строку)
            double ymath = Parser.Parse(_expr).Eval(_ctx);
            // удаить переменную из контекста
            _ctx.DeleteVariable("x");
            // возвратить вычисления.
            return ymath;
        }
    }
}
