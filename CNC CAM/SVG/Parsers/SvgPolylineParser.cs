using System.Collections.Generic;
using System.Windows;
using System.Xml;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers;

public class SvgPolylineParser<T>:SvgElementParser<T> where T:SvgPolyline, new()
{
    public override T Create(XmlElement element)
    {
        var polyLine = base.Create(element);
        double[] coordinates = element.GetAttribute("points").GetCommandArguments();
        List<Vector> points = new List<Vector>();
        for (int i = 0; i < coordinates.Length; i += 2)
        {
            points.Add(new Vector(coordinates[i], coordinates[i+1]));
        }
        polyLine.Points.AddRange(points);
        return polyLine;
    }
}