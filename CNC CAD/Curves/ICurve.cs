using System.Collections.Generic;
using System.Windows;
using CNC_CAD.Configs;

namespace CNC_CAD.Curves
{
    public interface ICurve
    {
        List<Vector> Linearize(AccuracySettings accuracy);
        public Transform Parent { get; set; }
        Vector StartPoint { get; }
        Vector EndPoint { get; }
        double Length { get; }
        public Vector ToGlobalPoint(Vector local);
    }
}