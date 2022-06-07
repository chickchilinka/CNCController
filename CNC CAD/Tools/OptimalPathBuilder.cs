using System;
using System.Collections.Generic;
using CNC_CAD.Curves;

namespace CNC_CAD.Tools;

public class OptimalPathBuilder<TTransform> where TTransform : Transform
{
    public static int MaxCycles = 1;
    private double _time = 0;
    private readonly Logger _logger;

    public OptimalPathBuilder()
    {
        _logger = Logger.CreateFor(this);
    }

    public List<TTransform> GetPathForTransforms(List<TTransform> transforms, out double distance)
    {
        PathMatrix<TTransform> matrix = new PathMatrix<TTransform>(transforms);
        distance = 0;
        for (int i = 0; i < transforms.Count - 1; i++)
            distance += transforms[i].GetDistanceTo(transforms[i + 1]) ?? 0;
        distance += transforms[^1].GetDistanceTo(transforms[0]) ?? 0;
        _logger.Log($"Old distance:{distance}");
        distance = 0;
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        _time = span.TotalMilliseconds;
        List<TTransform> path = ReplacementMethod(transforms);
        for (int i = 0; i < path.Count - 1; i++)
            distance += path[i].GetDistanceTo(path[i + 1]) ?? 0;
        distance += path[^1].GetDistanceTo(path[0]) ?? 0;
        TimeSpan span2 = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        _logger.Log($"Calculation time:{span2.TotalMilliseconds - _time} ms");
        _logger.Log($"New distance:{distance}");
        return path;
    }

    public void Swap(List<TTransform> points, int i, int j)
    {
        (points[i], points[j]) = (points[j], points[i]);
    }

    public List<TTransform> ReplacementMethod(List<TTransform> points)
    {
        List<TTransform> newPoints = new List<TTransform>(points);
        double oldDistance = 0;
        for (int s = 0; s < newPoints.Count - 1; s++)
            oldDistance += newPoints[s].GetDistanceTo(newPoints[s + 1]) ?? 0;
        oldDistance += newPoints[^1].GetDistanceTo(newPoints[0]) ?? 0;
        for (int k = 0; k < MaxCycles; k++)
        {
            bool replaced = false;
            for (int i = 0; i < newPoints.Count; i++)
            {
                for (int j = 0; j < newPoints.Count; j++)
                {
                    Swap(newPoints, i, j);
                    var distance = 0d;
                    for (int s = 0; s < newPoints.Count - 1; s++)
                        distance += newPoints[s].GetDistanceTo(newPoints[s + 1]) ?? 0;
                    distance += newPoints[^1].GetDistanceTo(newPoints[0]) ?? 0;
                    if (distance < oldDistance)
                    {
                        oldDistance = distance;
                        _logger.Log($"Iteration {k}:Swapped i:{i} element {newPoints[j]} and j:{j} element {newPoints[i]} distance = {distance}");
                        replaced = true;
                    }
                    else
                    {
                        Swap(newPoints, i, j);
                    }
                }
            }

            if (!replaced)
            {
                _logger.Log($"Finished solving at {k} iteration");
                return newPoints;
            }
        }

        return newPoints;
    }
}