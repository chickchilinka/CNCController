using System;
using System.Globalization;
using System.Xml;

namespace CNC_CAM.Tools;

public static class XmlExtensions
{
    public static double GetAttributeDouble(this XmlElement element,string attribute, double defaultValue = 0)
    {
        if (element.HasAttribute(attribute))
        {
            return Double.Parse(element.GetAttribute(attribute), CultureInfo.InvariantCulture);
        }
        else
        {
            return defaultValue;
        }
    }
}