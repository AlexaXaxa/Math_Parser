using Math_Parser_1._0.View.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Parser_1._0
{
    class Row
    {
        public RowType Type;
        public CustomTextBox Input;
        public OutputTextBox Output;
        public string VariableName;   // если Variable
        public PointF? Point;

    }
}
