using CNC_CAM.Shapes;

namespace CNC_CAM.SVG.Elements
{
    public abstract class SvgElement:Shape
    {
        public string Name { get; set; }
        public double StrokeWidth { get; set; }
        public bool Fill { get; set; }
    }
}