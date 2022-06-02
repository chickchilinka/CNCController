namespace CNC_CAD.Shapes
{
    public abstract class SvgElement:Shape
    {
        public double StrokeWidth { get; set; }
        public bool Fill { get; set; }
    }
}