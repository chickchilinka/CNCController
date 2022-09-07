using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Shapes;

namespace CNC_CAM.SVG.Subpaths;

public abstract class Subpath:Transform, ICurve
{
    public abstract List<Vector> Linearize(AccuracySettings accuracy);
    public abstract Vector StartPoint { get; }
    public abstract Vector EndPoint { get; }
    public abstract double Length { get; }
}