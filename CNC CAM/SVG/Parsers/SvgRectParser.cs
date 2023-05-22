using System;
using System.Globalization;
using System.Text;
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
        FormattableString topLine = $"M {x + rx},{y} h {w - rx*2} ";
        FormattableString rightTopArc = $"a {rx},{ry} 0 0 1 {rx},{ry} ";
        FormattableString rightLine = $"v {h - ry * 2} ";
        FormattableString rightBottomArc = $"a {rx},{ry} 0 0 1 {-rx},{ry} ";
        FormattableString bottomLine = $"h {-w + rx * 2} ";
        FormattableString bottomLeftArc = $"a {rx},{ry} 0 0 1 {-rx},{-ry} ";
        FormattableString leftLine = $"v {-h + ry * 2} ";
        FormattableString leftTopArc = $"a {rx},{ry} 0 0 1 {rx},{-ry} ";
        FormattableString.Invariant(topLine);
        StringBuilder result = new StringBuilder(FormattableString.Invariant(topLine));
        result.Append(rx>0? FormattableString.Invariant(rightTopArc):string.Empty);
        result.Append(FormattableString.Invariant(rightLine));
        result.Append(rx>0? FormattableString.Invariant(rightBottomArc):string.Empty);
        result.Append(FormattableString.Invariant(bottomLine));
        result.Append(rx>0? FormattableString.Invariant(bottomLeftArc):string.Empty);
        result.Append(FormattableString.Invariant(leftLine));
        result.Append(rx>0? FormattableString.Invariant(leftTopArc):string.Empty);

        return result.ToString();
    }
}