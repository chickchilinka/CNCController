using System;
using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.SVG.Subpaths
{
    public class SvgCubicBezier : Subpath
    {
        public Vector P0 { get; protected set; }
        public Vector P1 { get; protected set; }
        public Vector P2 { get; protected set; }
        public Vector P3 { get; protected set; }
        protected SvgCubicBezier continuation;
        public override Vector StartPoint => P0;
        public override Vector EndPoint => continuation?.EndPoint ?? P3;
        private double _length;
        public override double Length => _length + continuation?.Length ?? 0;

        public SvgCubicBezier(){}
        public SvgCubicBezier(double[] args, Vector start, bool relative = false)
        {
            P0 = start;
            P1 = new Vector(args[0], args[1]);
            P2 = new Vector(args[2], args[3]);
            P3 = new Vector(args[4], args[5]);
            if (relative)
            {
                P1 += start;
                P2 += start;
                P3 += start;
            }

            DetectContinuation(args, relative);
        }

        public virtual void DetectContinuation(double[] arguments, bool relative)
        {
            if (arguments.Length > 6)
            {
                if (arguments.Length % 6 != 0)
                {
                    throw new Exception("Invalid arguments count");
                }

                double[] continArgs = new double[arguments.Length - 6];
                Array.Copy(arguments, 6, continArgs, 0, arguments.Length - 6);
                continuation = new SvgCubicBezier(continArgs, P3, relative);
            }
        }
        public override Vector GetPointAt(double t)
        {
            return ToGlobalPoint(Math.Pow(1 - t, 3) * P0 + 3 * t * (1 - t) * (1 - t) * P1 + 3 * t * t * (1 - t) * P2 +
                                 t * t * t * P3);
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
            // points.Add(ToGlobalPoint(P3));
            // points = OptimizePoints(points, accuracy);
            if (continuation == null) return points;
            continuation.Parent = this.Parent;
            points.AddRange(continuation.Linearize(accuracy));

            return points;
        }

        public SvgCubicBezier GetLast()
        {
            return continuation?.GetLast() ?? this;
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

            points.Add(ToGlobalPoint(P3));
            return points;
        }
    }
}