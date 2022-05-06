using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAD.GCode;

namespace CNC_CAD.Shapes
{
    public class PathShape : Shape
    {
        private string _data;
        private string _name;

        public PathShape(string data, string name)
        {
            _data = data;
            _name = name;
            WpfShapes.Add(new Path()
            {
                Data = Geometry.Parse(data),
                Fill = Brushes.Transparent,
                StrokeThickness = 1d,
                Stroke = Brushes.Black
            });
        }

        public override List<GCodeCommand> GenerateGCodeCommands()
        {
            throw new NotImplementedException();
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