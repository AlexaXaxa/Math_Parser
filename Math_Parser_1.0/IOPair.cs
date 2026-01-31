using Math_Parser_1._0.View.UserControls;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Math_Parser_1._0
{
    public class IOPair
    {
        //??
        public Grid Container;
        public Input Input; 
        public Output Output; 
        public IOPair() 
        { 
            Input = new();
            Output = new("");
        } 
    }
}
