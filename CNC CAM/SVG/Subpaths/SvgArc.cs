using System;
using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.SVG.Subpaths
{
    /// <summary>
    /// Implementation of w3 SVG Path Arc
    /// used: https://mortoray.com/2017/02/16/rendering-an-svg-elliptical-arc-as-bezier-curves/
    /// </summary>
    public class SvgArc : Subpath
    {
        private readonly double _x1;
        private readonly double _y1;
        private readonly double _x2;
        private readonly double _y2;
        private readonly double _angle;
        private double _rx;
        private double _ry;
        private readonly bool _fa;
        private readonly bool _fs;
        private double _cx, _cy;
        private SvgArc continuation;
        private double length;
        public override double Length => length + continuation?.Length ?? 0;
        public override Vector StartPoint => new(_x1, _y1);
        public override Vector EndPoint => continuation?.EndPoint ?? new(_x2, _y2);


        public double Tetha1 { get; private set; }
        public double Dtetha { get; private set; }

        public SvgArc(double[] arguments, Vector startPoint, bool relative = false)
        {
            _x1 = startPoint.X;
            _y1 = startPoint.Y;
            _rx = arguments[0];
            _ry = arguments[1];
            _angle = arguments[2] * MathF.PI / 180f;
            _fa = (int)arguments[3] == 1;
            _fs = (int)arguments[4] == 1;
            _x2 = arguments[5];
            _y2 = arguments[6];
            if (relative)
            {
                _x2 += startPoint.X;
                _y2 += startPoint.Y;
            }

            DetectContinuation(arguments, relative);
            EndpointToCenterArcParams();
        }

        public void DetectContinuation(double[] arguments, bool relative)
        {
            if (arguments.Length > 7)
            {
                if (arguments.Length % 7 != 0)
                {
                    throw new Exception("Invalid arguments count");
                }

                double[] continArgs = new double[arguments.Length - 7];
                Array.Copy(arguments, 7, continArgs, 0, arguments.Length - 7);
                continuation = new SvgArc(continArgs, new Vector(_x2, _y2), relative);
            }
        }

        private void EndpointToCenterArcParams()
        {
            double rX = Math.Abs(_rx);
            double rY = Math.Abs(_ry);
            double dx2 = (_x1 - _x2) / 2.0;
            double dy2 = (_y1 - _y2) / 2.0;
            double x1p = Math.Cos(_angle) * dx2 + Math.Sin(_angle) * dy2;
            double y1p = -Math.Sin(_angle) * dx2 + Math.Cos(_angle) * dy2;
            double rxs = rX * rX;
            double rys = rY * rY;
            double x1ps = x1p * x1p;
            double y1ps = y1p * y1p;
            double cr = x1ps / rxs + y1ps / rys;
            if (cr > 1)
            {
                var s = Math.Sqrt(cr);
                rX = s * rX;
                rY = s * rY;
                rxs = rX * rX;
                rys = rY * rY;
            }

            double dq = (rxs * y1ps + rys * x1ps);
            double pq = (rxs * rys - dq) / dq;
            double q = Math.Sqrt(Math.Max(0, pq));
            if (_fa == _fs)
                q = -q;
            double cxp = q * rX * y1p / rY;
            double cyp = -q * rY * x1p / rX;
            double cx = Math.Cos(_angle) * cxp - Math.Sin(_angle) * cyp + (_x1 + _x2) / 2;
            double cy = Math.Sin(_angle) * cxp + Math.Cos(_angle) * cyp + (_y1 + _y2) / 2;
            double theta = SvgAngle(1, 0, (x1p - cxp) / rX, (y1p - cyp) / rY);
            double delta = SvgAngle(
                (x1p - cxp) / rX, (y1p - cyp) / rY,
                (-x1p - cxp) / rX, (-y1p - cyp) / rY);
            delta = delta % (Math.PI * 2);
            if (!_fs && delta > 0)
                delta -= 2 * Math.PI;
            else if (_fs && delta < 0)
                delta += 2 * Math.PI;
            _cx = cx;
            _cy = cy;
            Tetha1 = theta;
            Dtetha = delta;
        }

        static double SvgAngle(double ux, double uy, double vx, double vy)
        {
            var u = new Vector(ux, uy);
            var v = new Vector(vx, vy);
            //(F.6.5.4)
            var dot = u * v;
            var len = u.Length * v.Length;
            var ang = Math.Acos(Math.Clamp(dot / len, -1, 1)); //floating point precision, slightly over values appear
            if ((u.X * v.Y - u.Y * v.X) < 0)
                ang = -ang;
            return ang;
        }

        public override Vector GetPointAt(double relative)
        {
            var angleInRad = relative * Dtetha;
            var radFromStart = Tetha1 + angleInRad;
            var x = Math.Cos(_angle) * _rx * Math.Cos(radFromStart) - Math.Sin(_angle) * _ry * Math.Sin(radFromStart) +
                    _cx;
            var y = Math.Sin(_angle) * _rx * Math.Cos(radFromStart) + Math.Cos(_angle) * _ry * Math.Sin(radFromStart) +
                    _cy;
            return ToGlobalPoint(new Vector(x, y));
        }

        public override List<Vector> Linearize(double accuracy)
        {
            var points = new List<Vector>();
            var lastPoint = GetPointAt(0);
            points.Add(lastPoint);
            length = 0;
            // for (double i = Math.Sign(Dtetha)*accuracy.AngleAccuracy; Math.Abs(i) <= Math.Abs(Dtetha); i += Math.Sign(Dtetha)*accuracy.AngleAccuracy)
            // {
            //     points.Add(GetPointAt(i));
            //     length += (points[^1] - lastPoint).Length;
            // }
            points.AddRange(this.GetPointsBetween(0, 1, accuracy));
            points.Add(ToGlobalPoint(new Vector(_x2, _y2)));
            if (continuation == null) return points;
            continuation.Parent = this.Parent;
            points.AddRange(continuation.Linearize(accuracy));
            return points;
        }
    }
}