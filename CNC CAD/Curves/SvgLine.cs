using System.Collections.Generic;
using System.Windows;
using CNC_CAD.Configs;

namespace CNC_CAD.Curves
{
    public class SvgLine:Transform, ICurve
    {
        public Vector StartPoint { get; }
        public Vector EndPoint { get; }
        public SvgLine(double[] args, Vector startPoint, bool relative = false)
        {
            StartPoint = startPoint;
            if (!relative)
                EndPoint = new Vector(args[0], args[1]);
            else
                EndPoint = new Vector(startPoint.X + args[0], startPoint.Y + args[1]);
        }
        public List<Vector> Linearize(AccuracySettings accuracy)
        {
            return new List<Vector> {EndPoint};
        }
    }
}