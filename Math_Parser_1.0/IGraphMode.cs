using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Math_Parser_1._0
{
    public interface IGraphMode
    {
        public static Point downPoint;
        public static Point uppPoint;
        public static Cursor _customCursor = new Cursor("Assets/drag_cursor.cur");
        void OnMouseDown(Canvas name, MouseButtonEventArgs e);
        void OnMouseMove(Canvas name, MouseEventArgs e);
        void OnMouseUp(Canvas name, MouseButtonEventArgs e);
    }
}
