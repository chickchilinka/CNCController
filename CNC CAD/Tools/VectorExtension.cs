using System;
using System.Numerics;
using Vector = System.Windows.Vector;

namespace CNC_CAD.Tools
{
    public static class VectorExtension
    {
        public static double AngleBetween(Vector u, Vector v)
        {
            var sign = Math.Sign(u.X * v.Y - u.Y * v.X);
            var dotProduct = u * v;
            var a = dotProduct / (u.Length * v.Length);
            if (a < -1)
                a = -1;
            else if (a > 1)
                a = 1;
            return sign * Math.Acos(a);
        }
    }
}