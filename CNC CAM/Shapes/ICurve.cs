using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.SVG.Subpaths;

namespace CNC_CAM.Shapes
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