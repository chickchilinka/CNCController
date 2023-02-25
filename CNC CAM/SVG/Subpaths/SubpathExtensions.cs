using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.SVG.Subpaths;

public static class SubpathExtensions
{
    public static List<Vector> GetPointsBetween(this Subpath subpath, float start, float end, AccuracySettings settings)
    {
        var points = new List<Vector>();
        var startPoint = subpath.GetPointAt(start);
        var endPoint = subpath.GetPointAt(end);
        var lineCenter = startPoint + (endPoint - startPoint) / 2f;
        var curvePartCenter = subpath.GetPointAt((start + end) / 2f);
        if ((lineCenter - curvePartCenter).Length > settings.Accuracy)
        {
            points.AddRange(subpath.GetPointsBetween(start, start + (end - start) / 2, settings));
            points.Add(subpath.GetPointAt(start + (end - start) / 2));
            points.AddRange(subpath.GetPointsBetween(start + (end - start) / 2, end, settings));
        }
        return points;
    }
}