using System;
using System.Collections.Generic;
using System.Windows;
using CNC_CAD.Configs;

namespace CNC_CAD.Curves
{
    public class SvgLine : Transform, ICurve
    {
        public Vector StartPoint { get; }
        private Vector _endPoint;
        private SvgLine continuation;
        public Vector EndPoint => continuation?.EndPoint ?? _endPoint;

        public double Length => ToGlobalPoint(_endPoint - StartPoint).Length + continuation?.Length ?? 0;

        public enum Direction
        {
            Horizontal,
            Vertical,
            Both
        };

        public SvgLine(double[] args, Vector startPoint, Direction direction, bool relative = false)
        {
            StartPoint = startPoint;
            double x = direction == Direction.Both || direction == Direction.Horizontal
                ? (relative ? args[0] + startPoint.X : args[0])
                : startPoint.X;
            double y = 0;
            int yArgId = direction == Direction.Both ? 1 : 0;
            if (direction == Direction.Both || direction == Direction.Vertical)
            {
                y = relative ? args[yArgId] + startPoint.Y : args[yArgId];
            }
            else
            {
                y = startPoint.Y;
            }

            _endPoint = new Vector(x, y);
            DetectContinuation(args, relative, direction);
        }

        public void DetectContinuation(double[] arguments, bool relative, Direction direction)
        {
            if (direction == Direction.Both)
            {
                if (arguments.Length > 2)
                {
                    if (arguments.Length % 2 != 0)
                    {
                        throw new Exception("Invalid arguments count");
                    }

                    double[] continArgs = new double[arguments.Length - 2];
                    Array.Copy(arguments, 2, continArgs, 0, arguments.Length - 2);
                    continuation = new SvgLine(continArgs, _endPoint, direction, relative);
                }
            }
            else
            {
                if (arguments.Length > 1)
                {
                    double[] continArgs2 = new double[arguments.Length - 1];
                    Array.Copy(arguments, 1, continArgs2, 0, arguments.Length - 1);
                    continuation = new SvgLine(continArgs2, _endPoint, direction, relative);
                }
            }
        }

        public List<Vector> Linearize(AccuracySettings accuracy)
        {
            var result = new List<Vector> { ToGlobalPoint(_endPoint) };
            if (continuation == null) return result;
            continuation.Parent = this.Parent;
            result.AddRange(continuation.Linearize(accuracy));
            return result;
        }
    }
}