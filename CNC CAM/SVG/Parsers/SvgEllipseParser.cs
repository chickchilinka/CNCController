using System.Xml;
using CNC_CAM.Tools;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers;

public class SvgEllipseParser:SvgPathDataParser
{
    public override SvgEllipse Create(XmlElement element)
    {
        double cx = element.GetAttributeDouble("cx");
        double cy = element.GetAttributeDouble("cy");
        double rx = element.GetAttributeDouble("rx");
        double ry = element.GetAttributeDouble("ry");
        double r = element.GetAttributeDouble("r");
        if (r > 0)
        {
            ry = r;
            rx = r;
        }
        if (ry == 0)
        {
            ry = rx;
        }
        else if (rx == 0)
        {
            rx = ry;
        }
        var pathData = $"M {cx-rx},{cy} a {rx},{ry} 0 1 0 {rx*2},0 a {rx},{ry} 0 1 0 {-rx*2},0";
        var curves = GetCurves(pathData);
        return new SvgEllipse(pathData, GetId(element), curves)
        {
            TransformationMatrix = element.GetTransformationMatrix()
        };
    }
}