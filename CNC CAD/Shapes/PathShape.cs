using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.GCode;

namespace CNC_CAD.Shapes
{
    public class PathShape : Shape
    {
        private string _data;
        public string Data
        {
            get => _data;
            private init => _data = value;
        }

        private string _name;

        public PathShape(string data, string name)
        {
            Data = data;
            _name = name;
            var geometry = Geometry.Parse(data);
            WpfShapes.Add(new Path()
            {
                Data = Geometry.Parse(data),
                Fill = Brushes.Transparent,
                StrokeThickness = 1d,
                Stroke = Brushes.Black
            });
        }

        public override List<GCodeCommand> GenerateGCodeCommands(CncConfig config)
        {
            return new List<GCodeCommand>
            {
                new(GCodeBuilder2D.ForPath(config, Data)
                    .SetHeadDownAtStart(false)
                    .SetHeadDownAtEnd(true)
                    .Build())
            };
        }


        public override void Move(Vector2 delta)
        {
            throw new NotImplementedException();
        }

        public override void Scale(Vector2 multiplication, Vector2 pivot)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(float angle, Vector2 pivot)
        {
            throw new NotImplementedException();
        }
    }
}