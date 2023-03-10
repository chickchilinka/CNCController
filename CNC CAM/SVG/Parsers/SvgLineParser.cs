using System.Collections.Generic;
using System.Windows;
using System.Xml;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers;

public class SvgLineParser : SvgElementParser<SvgPolyline>
{
    public override SvgPolyline Create(XmlElement element)
    {
        var polyline = base.Create(element);
        var x1 = GetAttribute(element, "x1") ?? 0;
        var y1 = GetAttribute(element, "y1") ?? 0;
        var x2 = GetAttribute(element, "x2") ?? 0;
        var y2 = GetAttribute(element, "y2") ?? 0;
        var points = new List<Vector> { new Vector(x1, y1), new Vector(x2, y2) };
        polyline.Points.AddRange(points);
        return polyline;
    }

    private static double? GetAttribute(XmlElement element, string name)
    {
        if (double.TryParse(element.GetAttribute(name), out var value))
        {
            return value;
        }

        return null;
    }
}