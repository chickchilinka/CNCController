using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Shapes;

namespace CNC_CAM.SVG.Elements;

public class SvgRect:SvgPath
{
    public SvgRect()
    {
        
    }
    public SvgRect(string data, string name, List<ICurve> curves,  Vector? end = null):base(data, name, curves, end)
    {
        
    }

}