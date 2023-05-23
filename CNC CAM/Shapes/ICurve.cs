using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration.Data;
using CNC_CAM.SVG.Subpaths;

namespace CNC_CAM.Shapes
{
    public interface ICurve
    {
        List<Vector> Linearize(double accuracy);
        public Transform Parent { get; set; }
        Vector StartPoint { get; }
        Vector EndPoint { get; }
        double Length { get; }
        public Vector ToGlobalPoint(Vector local);
    }
}