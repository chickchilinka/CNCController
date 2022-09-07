using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.SVG.Elements;

public class SvgPolygon:SvgPolyline
{
    private Polygon _wpfPolygon; 
    public SvgPolygon()
    {
        _wpfPolygon = new Polygon()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        WpfShapes.Add(_wpfPolygon);
    }
    
    public override List<Shape> GetControlShapes()
    {
        _wpfPolygon.Points = new PointCollection(_points.Select(vector => new Point(vector.X, vector.Y)));
        return WpfShapes;
    }

    public override List<Vector> Linearize(AccuracySettings accuracySettings)
    {
        List<Vector> points = base.Linearize(accuracySettings);
        if(points.Count>0)
            points.Add(points[0]);
        return points;
    }
}