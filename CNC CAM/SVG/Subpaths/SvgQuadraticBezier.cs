using System;
using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.SVG.Subpaths
{
    public class SvgQuadraticBezier : Subpath
    {
        private Vector P0;
        private Vector P1;
        private Vector P2;
        private SvgQuadraticBezier continuation;
        public override Vector StartPoint => P0;
        public override Vector EndPoint => continuation?.EndPoint ?? P2;
        private double _length;
        public override double Length => _length + continuation?.Length ?? 0;

        public SvgQuadraticBezier(double[] args, Vector _start, bool relative = false)
        {
            P0 = _start;
            P1 = new Vector(args[0], args[1]);
            P2 = new Vector(args[2], args[3]);
            if (relative)
            {
                P1 += _start;
                P2 += _start;
            }

            DetectContinuation(args, relative);
        }

        public void DetectContinuation(double[] arguments, bool relative)
        {
            if (arguments.Length > 4)
            {
                if (arguments.Length % 4 != 0)
                {
                    throw new Exception("Invalid arguments count");
                }

                double[] continArgs = new double[arguments.Length - 4];
                Array.Copy(arguments, 4, continArgs, 0, arguments.Length - 4);
                continuation = new SvgQuadraticBezier(continArgs, P2, relative);
            }
        }

        public SvgQuadraticBezier(Vector p0, Vector p1, Vector p2)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
        }

        public override Vector GetPointAt(double t)
        {
            return ToGlobalPoint(Math.Pow(1 - t, 2) * P0 + 2 * t * (1 - t) * P1 + t * t * P2);
        }

        public SvgQuadraticBezier AsContinuation(double[] args)
        {
            Vector p2 = new Vector(args[0], args[1]);
            Vector p1 = (P2 - P1) + P2;
            Vector p0 = P2;
            return new SvgQuadraticBezier(p0, p1, p2);
        }

        public override List<Vector> Linearize(AccuracySettings accuracy)
        {
            var points = new List<Vector>() { GetPointAt(0) };
            points.AddRange(this.GetPointsBetween(0, 1, accuracy));
            points.Add(GetPointAt(1));
            // for (double i = 0; i <= 1d; i += accuracy.RelativeAccuracy)
            // {
            //     points.Add(GetPointAt(i));
            // }
            //
            // points.Add(ToGlobalPoint(P2));
            // points = OptimizePoints(points, accuracy);
            if (continuation == null) return points;
            continuation.Parent = this.Parent;
            points.AddRange(continuation.Linearize(accuracy));

            return points;
        }

        private List<Vector> OptimizePoints(List<Vector> points, AccuracySettings accuracySettings)
        {
            double sum = (ToGlobalPoint(StartPoint) - points[0]).Length;
            for (int i = 1; i < points.Count; i++)
            {
                sum += (points[i] - points[i - 1]).Length;
            }

            double stepSize = accuracySettings.Accuracy / (sum * 10);
            points.Clear();
            for (double i = 0; i <= 1d; i += stepSize)
            {
                points.Add(GetPointAt(i));
            }

            points.Add(ToGlobalPoint(P2));
            return points;
        }
    }
}