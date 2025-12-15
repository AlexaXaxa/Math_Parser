using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Math_Parser_1._0.View.UserControls
{
    /// <summary>
    /// Interaction logic for OutputTextBox.xaml
    /// </summary>
    public partial class OutputTextBox : UserControl
    {
       
        public OutputTextBox(string text)
        {
            InitializeComponent();
            SetText(text);
        }

        public void SetText(string text)
        {
            tblPlaceholder.Text = text;
        }
    }
}
