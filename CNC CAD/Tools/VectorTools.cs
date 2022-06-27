using System.Windows;

namespace CNC_CAD.Tools;

public static class VectorTools
{
    public static Vector ToVector(this Point point)
    {
        return new Vector(point.X, point.Y);
    }
}