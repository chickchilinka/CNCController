using System;
using System.Windows;
using System.Windows.Media;

namespace CNC_CAM.Tools;

public static class MatrixTools
{
    public static double ExtractRotationInAngles(this Matrix matrix)
    {
        if (matrix.ExtractScale().X == 0 || matrix.ExtractScale().Y==0)
            return 0;
        return Math.Atan2(-matrix.M12, matrix.M11) * 180 / Math.PI;
    }

    public static Vector ExtractScale(this Matrix matrix)
    {
        return new Vector(Math.Sign(matrix.M11)*new Vector(matrix.M11,matrix.M21).Length, Math.Sign(matrix.M22)*new Vector(matrix.M12 , matrix.M22).Length);
    }

    public static Vector ExtractTranslation(this Matrix matrix)
    {
        return new(matrix.OffsetX, matrix.OffsetY);
    }
}