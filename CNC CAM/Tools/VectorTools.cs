using System.Windows;

namespace CNC_CAM.Tools;

public static class VectorTools
{
    public static Vector ToVector(this Point point)
    {
        return new Vector(point.X, point.Y);
    }
}