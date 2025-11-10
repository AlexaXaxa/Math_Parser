using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Math_Parser_1._0.View.UserControls.GraphControl;

namespace Math_Parser_1._0
{
    class DrawPolygonMode : IGraphMode
    {
        LinkedList<Point> Polygon = new LinkedList<Point>();
        Point lastpoint;
        double clickRadius = 10;


        public void OnMouseDown(Canvas name, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(name);
            if (Polygon.Count == 0)
            {
                Point newPoint = new Point(position.X, position.Y);
                Polygon.AddLast(newPoint);
                figures.Add(CreatePoint(e, name));
                lastpoint = newPoint;

            }
            else
            {
                int i = figures.Count;
                //kopia för att iterera och ändra inte storlek på Polygon
                LinkedList<Point> PolygonTest = new LinkedList<Point>(Polygon);
                foreach (var p in PolygonTest)
                {
                    double dx = position.X - p.X;
                    double dy = position.Y - p.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);


                    //if point is set to other position than existing point
                    if (distance >= clickRadius)
                    {

                        Point newPoint = new Point(position.X, position.Y);
                        Polygon.AddLast(newPoint);
                       
                        figures.Add(CreatePoint(e, name));

                        Line Line = new Line();
                        Line.X1 = lastpoint.X;
                        Line.Y1 = lastpoint.Y;
                        Line.X2 = newPoint.X;
                        Line.Y2 = newPoint.Y;

                        Color myRgbColor = new Color();
                        myRgbColor = Color.FromRgb(181, 107, 70);
                       

                        Line.Stroke = new SolidColorBrush(myRgbColor);
                        Line.StrokeThickness = 3;

                        figures.Add(new GraphSegment("PolygonSegment", "Segment", Line, Line));
                        i = figures.Count;
                        lastpoint = newPoint;
                    }
                   
                    
                    else //if point is set to existing point
                    {

                        Line Line = new Line();
                        Line.X1 = lastpoint.X;
                        Line.Y1 = lastpoint.Y;
                        Line.X2 = p.X;
                        Line.Y2 = p.Y;

                        Color myRgbColor = new Color();
                        myRgbColor = Color.FromRgb(181, 107, 70);
                        Line.Stroke = new SolidColorBrush(myRgbColor);
                        Line.StrokeThickness = 3;

                        figures.Add(new GraphSegment("PolygonSegment", "Segment", Line, Line));

                       


                        //Add the Polygon Element
                        var newPolygon = new Polygon();
                       
                        
                        
                        SolidColorBrush strokeBrush = new SolidColorBrush(myRgbColor);
                        strokeBrush.Opacity = .15d;
                        newPolygon.Fill = strokeBrush;

                        PointCollection myPointCollection = new PointCollection();


                        foreach (var point in Polygon)
                        {
                            myPointCollection.Add(point);
                        }

                      
                        newPolygon.Points = myPointCollection;

                        
                        figures.Add(new GraphPolygon("Polygon", "Polygon", newPolygon, myPointCollection, newPolygon));
                        
                        Polygon.Clear();
                        break;
                    }
                }
            }
              
            
           
           

        }

        public void OnMouseMove(Canvas name, MouseEventArgs e)
        {
           
        }

        public void OnMouseUp(Canvas name, MouseButtonEventArgs e)
        {
           
        }
    }
}
