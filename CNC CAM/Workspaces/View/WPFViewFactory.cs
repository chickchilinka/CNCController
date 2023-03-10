using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.Workspaces.View;

public class WPFViewFactory
{
    public Dictionary<SvgElement, Shape> GetShapes(SvgGroupElement svgGroup)
    {
        Dictionary<SvgElement, Shape> shapes = new();
        foreach (var element in svgGroup.Children)
        {
            if(element is SvgGroupElement childGroup)
                GetShapes(childGroup).ToList().ForEach(pair=>shapes.Add(pair.Key, pair.Value));
            else
                shapes.Add(element, GetShapeView(element));
        }

        return shapes;
    }

    private Shape GetShapeView(SvgElement element)=>
        element switch
        {
            SvgPath svgPath => GetPath(svgPath),
            SvgPolygon svgPolygon => GetPolygon(svgPolygon),
            SvgPolyline svgPolyline => GetPolyline(svgPolyline),
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };

    private Path GetPath(SvgPath path)
    {
        return new Path()
        {
            Data = Geometry.Parse(path.Data),
            Fill = Brushes.Transparent,
            StrokeThickness = 1d,
            Stroke = Brushes.Black
        };
    }

    private Polygon GetPolygon(SvgPolygon polygon)
    {
        return new Polygon()
        {
            Stroke = Brushes.Black,
            Points = new PointCollection(polygon.Points.Select(vector => new Point(vector.X, vector.Y))),
            StrokeThickness = 1
        };
    }

    private Polyline GetPolyline(SvgPolyline polyline)
    {
        return new Polyline()
        {
            Points = new PointCollection(polyline.Points.Select(vector => new Point(vector.X, vector.Y))), 
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
    }
}