using System.Windows.Controls;
using System.Windows.Input;
using static Math_Parser_1._0.View.UserControls.GraphControl;



namespace Math_Parser_1._0
{

    class MoveGridMode : IGraphMode
    {


        public void OnMouseDown(Canvas g, MouseButtonEventArgs e)
        {

            g.Cursor = IGraphMode._customCursor;

            IGraphMode.downPoint = e.GetPosition(g);
            double x = IGraphMode.downPoint.X;
            double y = IGraphMode.downPoint.Y;

        }

        public void OnMouseMove(Canvas name, MouseButtonEventArgs e)
        {
            // Implementation goes here
        }

        public void OnMouseUp(Canvas g, MouseButtonEventArgs e)
        {
            g.Cursor = Cursors.Arrow;

            IGraphMode.uppPoint = e.GetPosition(g);
            double x = IGraphMode.downPoint.X;
            double y = IGraphMode.downPoint.Y;

            Math_Parser_1._0.View.UserControls.GraphControl.CalculateOffset(IGraphMode.downPoint, IGraphMode.uppPoint);
            /*тут считаются начальные координаты и конечные, дельта. И высчитываются публичные
             переменные offsetY, offsetX
            То есть мне по идее надо двигать И оси И сетку на расстояние offsetY, offsetX.
            Оси:
            их я рисую сначала по середине. это начальное положение. потом к этому начальному 
            положению я прибавляю offsetY, offsetX каждый раз когда я двигаю оси 
            (использую MoveGridMode)
            
            Grid:
            тут сложнее. Сейчас конкретно я рисую вертикальные оси от начала контрола до 
            его конца и горизонтальные от верха контрола до его низа.
            когда я двигаю сетку (использую MoveGridMode) мне нужно рисовать оси так же от 
            начала экрана до конца. Я сделала ошибку потому что использовала значение конца уменьше
            нное или увеличенное. Это не правильно. сетка всегда тянется от начала и до конца, она
            видна ВСЕГДА как бы далеко мы не двигали график.
            То что нужно менять это то от куда рисуется наша сетка. от какого положения оси

            
            
            
            */
        }
    }
}
