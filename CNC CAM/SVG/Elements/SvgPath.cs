using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Shapes;
using Transform = CNC_CAM.SVG.Subpaths.Transform;
using Vector = System.Windows.Vector;

namespace CNC_CAM.SVG.Elements
{
    public class SvgPath : SvgElement, ICurve
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

        public SvgPath()
        {
            
        }
        public SvgPath(string data, string name, List<ICurve> curves, Vector? end = null)
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

        public override double? GetDistanceTo(Subpaths.Transform transform)
        {
            if (transform == this)
                return Double.MaxValue;
            if (transform is SvgPath shape)
            {
                return (EndPoint - shape.StartPoint).Length;
            }
            return null;
        }

        public void Concat(SvgPath shape2)
        {
            WpfShapes.AddRange(shape2.WpfShapes);
            Curves.AddRange(shape2.Curves);
        }
    }
}