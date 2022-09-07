using System.Collections.Generic;

namespace CNC_CAM.SVG.Elements
{
    public class SvgRoot:SvgGroupElement
    {
        public static void ConcatShapes(List<SvgPath> shapes)
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