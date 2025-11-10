using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        
        public void OnMouseDown(Canvas name, MouseButtonEventArgs e)
        {
            CreateSegment(e, name);


            

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
