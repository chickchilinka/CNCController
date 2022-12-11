using System.Collections.Generic;
using System.Xml;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers;

public class SvgParsersFactory
{
    private Dictionary<string, SvgElementParser> _parsers = new();
    private static SvgParsersFactory _instance = new(); 
    public SvgParsersFactory()
    {
        RegisterParser<SvgGroupDataParser<SvgGroupElement>>("g");
        RegisterParser<SvgParser>("svg");
        RegisterParser<SvgPathDataParser>("path");
        RegisterParser<SvgPolylineParser<SvgPolyline>>("polyline");
        RegisterParser<SvgLineParser>("line");
        RegisterParser<SvgPolylineParser<SvgPolygon>>("polygon");
        RegisterParser<SvgRectParser>("rect");
        RegisterParser<SvgEllipseParser>("ellipse");
        RegisterParser<SvgEllipseParser>("circle");
    }

    public static SvgElement Parse(XmlElement element)
    {
        if (_instance._parsers.ContainsKey(element.Name))
        {
            return _instance._parsers[element.Name].Create(element);
        }

        return null;
    }
    
    private void RegisterParser<T>(string elementTag) where T : SvgElementParser, new()  
    {
        _parsers.Add(elementTag, new T());
    }
}