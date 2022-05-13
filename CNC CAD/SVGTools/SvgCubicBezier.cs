using System;
using System.Windows;

namespace CNC_CAD.SvgTools
{
    public class SvgCubicBezier
    {
        private Vector P0;
        private Vector P1;
        private Vector P2;
        private Vector P3;
        
        public SvgCubicBezier(double[] args, Vector _start)
        {
            P0 = _start;
            P1 = new Vector(args[0], args[1]);
            P2 = new Vector(args[2], args[3]);
            P3 = new Vector(args[4], args[5]);
        }
        
        public SvgCubicBezier(Vector p0, Vector p1, Vector p2, Vector p3)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public Vector GetPointAt(double t)
        {
            return Math.Pow(1 - t, 3) * P0 + 3 * t * (1 - t) * (1 - t) * P1 + 3 * t * t * (1 - t) * P2 + t * t * t * P3;
        }

        public SvgCubicBezier AsContinuation(double[] args)
        {
            Vector p2 = new Vector(args[0], args[1]);
            Vector p3 = new Vector(args[1], args[2]);
            Vector p0 = P3;
            Vector p1 = (P3 - P2) + P3;
            return new SvgCubicBezier(p0, p1, p2, p3);
        }
    }
}