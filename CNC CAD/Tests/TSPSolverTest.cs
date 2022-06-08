using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CNC_CAD.Curves;
using CNC_CAD.CustomWPFElements;
using CNC_CAD.Shapes;
using CNC_CAD.Tools;

namespace CNC_CAD.Tests
{
    public class TSPSolverTest : ITest
    {
        private Logger _logger;
        private int count = 200;
        private List<Point> points = new List<Point>();
        OptimalPathBuilder<Point> pathBuilder = new OptimalPathBuilder<Point>();
        public Workspace2D _workspace2D { get; set; }

        private class Point : Transform
        {
            public int Id { get; }
            public Vector Position { get; }

            public Point(int id, Vector position)
            {
                Id = id;
                Position = position;
            }

            public override double? GetDistanceTo(Transform otherPoint)
            {
                if (otherPoint == this)
                    return null;
                if (otherPoint is Point point)
                    return (Position - point.Position).Length;
                return null;
            }

            public override string ToString()
            {
                return Id.ToString();
            }
        }

        public TSPSolverTest()
        {
            _logger = Logger.CreateFor(this);
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                Point point = new(i, new(random.Next(0, 1000), random.Next(0, 1000)));
                points.Add(point);
            }
        }

        private void LaunchTest()
        {
            _workspace2D.ClearShapes();
            double sum = 0;
            var path = pathBuilder.GetPathForTransforms(points, out sum);
            new Action(async () =>
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var p = path[i];
                    if (_workspace2D != null && i < path.Count - 1)
                    {
                        PathShape shape =
                            new PathShape(
                                $"M {p.Position.X} {p.Position.Y} L {path[i + 1].Position.X} {path[i + 1].Position.Y}",
                                i.ToString(), new List<ICurve>());
                        _workspace2D.AddShapes(shape.GetControlShapes().ToArray());
                    }

                    await Task.Delay(100);
                    _logger.Log(p);
                }
            }).Invoke();
            _logger.Log("length:" + sum);
            _workspace2D.InvalidateVisual();
            OptimalPathBuilder<Point>.MaxCycles += 10;
        }

        public void Test()
        {
            LaunchTest();
        }
    }
}