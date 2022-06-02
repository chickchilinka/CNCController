using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.GCode;
using Vector = System.Windows.Vector;

namespace CNC_CAD.Shapes
{
    public class PathShape : SvgElement, ICurve
    {
        private string _data;
        public string Data
        {
            get => _data;
            private init => _data = value;
        }

        private string _name;
        public List<ICurve> Curves { get; }
        public Vector StartPoint { get; set; }
        public Vector EndPoint { get; set; }

        public PathShape(string data, string name, List<ICurve> curves)
        {
            Data = data;
            _name = name;
            WpfShapes.Add(new Path()
            {
                Data = Geometry.Parse(data),
                Fill = Brushes.Transparent,
                StrokeThickness = 1d,
                Stroke = Brushes.Black
            });
            Curves = curves;
        }

        public override List<GCodeCommand> GenerateGCodeCommands(CncConfig config)
        {
            return new List<GCodeCommand>
            {
                new(GCodeBuilder2D.ForPath(config, this)
                    .SetHeadDownAtStart(false)
                    .SetHeadDownAtEnd(true)
                    .Build())
            };
        }
        
        public List<Vector> Linearize(AccuracySettings accuracy)
        {
            return null;
        }

        public override string ToString()
        {
            return $"{_name} startPoint:{StartPoint}, endPoint:{EndPoint}";
        }
    }
}