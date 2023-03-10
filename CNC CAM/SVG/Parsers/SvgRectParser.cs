using System;
using System.Xml;
using CNC_CAM.Tools;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers;

public class SvgRectParser:SvgPathDataParser
{
    public SvgRectParser()
    {
    }

    public override SvgRect Create(XmlElement element)
    {
        double x = element.GetAttributeDouble("x");
        double y = element.GetAttributeDouble("y");
        double w = element.GetAttributeDouble("width");
        double h = element.GetAttributeDouble("height");
        double rx = element.GetAttributeDouble("rx");
        double ry = element.GetAttributeDouble("ry");
        if (ry == 0)
        {
            ry = rx;
        }
        else if (rx == 0)
        {
            rx = ry;
        }
        var pathData = GetPathDataAnalog(x, y, w, h, rx, ry);
        var curves = GetCurves(pathData);
        return new SvgRect(pathData, GetId(element), curves) {
            TransformationMatrix = element.GetTransformationMatrix()
        };
    }
    
    
    public string GetPathDataAnalog(double x, double y, double w, double h, double rx, double ry)
    {
        var result = $"M {x + rx},{y} h {w - rx*2} ";
        result+= rx>0? $"a {rx},{ry} 0 0 1 {rx},{ry} ":string.Empty;
        result+= $"v {h-ry*2} ";
        result += rx>0? $"a {rx},{ry} 0 0 1 {-rx},{ry} ":string.Empty;
        result+= $"h {-w + rx*2} ";
        result+= rx>0? $"a {rx},{ry} 0 0 1 {-rx},{-ry} ":string.Empty;
        result+= $"v {-h+ry*2} ";
        result+= rx>0? $"a {rx},{ry} 0 0 1 {rx},{-ry} ":string.Empty;
        return result ;
    }
}