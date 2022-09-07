using System;
using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.SVG.Subpaths
{
    public class SvgCubicBezier : Subpath
    {
        private Vector P0;
        private Vector P1;
        private Vector P2;
        private Vector P3;
        private SvgCubicBezier continuation;
        public override Vector StartPoint => P0;
        public override Vector EndPoint  => continuation?.EndPoint ?? P3;
        private double _length;
        public override double Length => _length + continuation?.Length ?? 0;
        
        public SvgCubicBezier(double[] args, Vector _start, bool relative = false)
        {
            P0 = _start;
            P1 = new Vector(args[0], args[1]);
            P2 = new Vector(args[2], args[3]);
            P3 = new Vector(args[4], args[5]);
            if (relative)
            {
                P1 += _start;
                P2 += _start;
                P3 += _start;
            }
            DetectContinuation(args, relative);
        }
        
        public void DetectContinuation(double[] arguments, bool relative)
        {
            if (arguments.Length > 6 )
            {
                if (arguments.Length % 6 != 0)
                {
                    throw new Exception("Invalid arguments count");
                }
                double[] continArgs = new double[arguments.Length-6];
                Array.Copy(arguments, 6, continArgs, 0, arguments.Length-6);
                continuation = new SvgCubicBezier(continArgs, P3, relative);
            }
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
            return ToGlobalPoint(Math.Pow(1 - t, 3) * P0 + 3 * t * (1 - t) * (1 - t) * P1 + 3 * t * t * (1 - t) * P2 +
                                 t * t * t * P3);
        }

        public SvgCubicBezier AsContinuation(double[] args)
        {
            Vector p2 = new Vector(args[0], args[1]);
            Vector p3 = new Vector(args[1], args[2]);
            Vector p0 = P3;
            Vector p1 = (P3 - P2) + P3;
            return new SvgCubicBezier(p0, p1, p2, p3);
        }

        public override List<Vector> Linearize(AccuracySettings accuracy)
        {
            var points = new List<Vector>();
            for (double i = 0; i <= 1d; i += accuracy.RelativeAccuracy)
            {
                points.Add(GetPointAt(i));
            }
            points.Add(ToGlobalPoint(P3));
            points = OptimizePoints(points, accuracy);
            if (continuation == null) return points;
            continuation.Parent = this.Parent;
            points.AddRange(continuation.Linearize(accuracy));

            return points;
        }
        
        private List<Vector> OptimizePoints(List<Vector> points, AccuracySettings accuracySettings)
        {
            double sum = (ToGlobalPoint(StartPoint)-points[0]).Length;
            for (int i = 1; i < points.Count; i++)
            {
                sum += (points[i] - points[i - 1]).Length;
            }

            double stepSize = accuracySettings.AccuracyPer10MM/(sum*10);
            points.Clear();
            for (double i = 0; i <= 1d; i += stepSize)
            {
                points.Add(GetPointAt(i));
            }
            points.Add(ToGlobalPoint(P3));
            return points;
        }
    }
}