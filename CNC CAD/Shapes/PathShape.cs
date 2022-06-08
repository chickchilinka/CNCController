using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.GCode;
using Transform = CNC_CAD.Curves.Transform;
using Vector = System.Windows.Vector;

namespace CNC_CAD.Shapes
{
    public class PathShape : SvgElement, ICurve
    {
        private string _data;
        public string Data
        {
            get => _data;
            set => _data = value;
        }
        
        public List<ICurve> Curves { get; }
        private Vector? _start;
        private Vector? _end;
        public double Length { get; }

        public Vector StartPoint
        {
            get
            {
                if (Curves.Count > 0)
                    return Curves[0].StartPoint;
                return _start ?? default;
            }
            set => _start = value;
        }

        public Vector EndPoint
        {
            get
            {
                if (Curves.Count > 0 && _end==null)
                    return Curves[^1].EndPoint;
                return _end ?? default;
            }
            set => _start = value;
        }

        public PathShape()
        {
            
        }
        public PathShape(string data, string name, List<ICurve> curves, Vector? end = null)
        {
            Data = data;
            Name = name;
            WpfShapes.Add(new Path()
            {
                Data = Geometry.Parse(data),
                Fill = Brushes.Transparent,
                StrokeThickness = 1d,
                Stroke = Brushes.Black
            });
            Curves = curves;
            _end = end;
            SetParentToCurves();
        }
        
        public void SetParentToCurves()
        {
            foreach (var curve in Curves)
            {
                curve.Parent = this;
            }
        }

        public override List<GCodeCommand> GenerateGCodeCommands(CncConfig config)
        {
            return new List<GCodeCommand>
            {
                new(GCodeBuilder2D.ForPath(config, this)
                    .SetHeadDownAtStart(false)
                    .SetHeadDownAtEnd(false)
                    .Build())
            };
        }
        
        public List<Vector> Linearize(AccuracySettings accuracy)
        {
            return null;
        }

        public override string ToString()
        {
            return $"{Name} startPoint:{StartPoint}, endPoint:{EndPoint}";
        }

        public override double? GetDistanceTo(Transform transform)
        {
            if (transform == this)
                return Double.MaxValue;
            if (transform is PathShape shape)
            {
                return (EndPoint - shape.StartPoint).Length;
            }
            return null;
        }

        public void Concat(PathShape shape2)
        {
            WpfShapes.AddRange(shape2.WpfShapes);
            Curves.AddRange(shape2.Curves);
        }
        
    }
}