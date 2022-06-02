using System.Collections.Generic;
using System.Windows;
using CNC_CAD.Configs;

namespace CNC_CAD.Curves
{
    public interface ICurve
    {
        List<Vector> Linearize(AccuracySettings accuracy);
        Vector StartPoint { get; }
        Vector EndPoint { get; }
    }
}