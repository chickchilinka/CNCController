using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using CNC_CAD.Configs;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;
using CNC_CAD.Tools;

namespace CNC_CAD.Shapes
{
    public class SvgRoot:SvgGroupElement
    {
        public static void ConcatShapes(List<PathShape> shapes)
        {
            bool found = false;
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i] != null)
                {
                    for (int j = 0; j < shapes.Count; j++)
                    {
                        if (i != j && shapes[j] != null && shapes[j].StartPoint == shapes[i].EndPoint)
                        {
                            found = true;
                            shapes[i].Concat(shapes[j]);
                            shapes.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            if (found)
            {
                ConcatShapes(shapes);
            }
        }
    }
}