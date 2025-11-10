using Math_Parser_1._0.View.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Math_Parser_1._0.View.UserControls.CustomTextBox;
using static Math_Parser_1._0.View.UserControls.GraphControl;


namespace Math_Parser_1._0
{
    class DrawPointMode : IGraphMode
    {
        public static List<char> PointAlphabet = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Z' };
        public static List<char> HelpPointAlphabet = new List<char>(PointAlphabet);

        public static int PointAlphabetNum = 0;

      

     
        public string OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {
            var newPoint = CreatePoint(e, g);
            figures.Add(newPoint);

            string pointName = NextPointName(); // A, B, C...

            string pointText = $"{pointName} = ({newPoint.Position.X:F1}, {newPoint.Position.Y:F1})";



            return pointText;
          
           


        }

        public void OnMouseMove(Canvas g, MouseEventArgs e)
        {
            // Implementation goes here
        }

        public void OnMouseUp(Canvas g, MouseButtonEventArgs e)
        {
            // Implementation goes here
        }

        public static string NextPointName()
        {
            if(HelpPointAlphabet.Count < 1)
            {
                PointAlphabetNum += 1;
                HelpPointAlphabet = new List<char>(PointAlphabet);

                var i = HelpPointAlphabet.First();
                HelpPointAlphabet.RemoveAt(0);

                return i.ToString() + PointAlphabetNum.ToString();
            }
            else
            {
                var i = HelpPointAlphabet.First();
                HelpPointAlphabet.RemoveAt(0);

                return i.ToString() + PointAlphabetNum.ToString();
            }
            
        }

    }
}
