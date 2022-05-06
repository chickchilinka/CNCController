using System.Collections.Generic;
using System.Numerics;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;

namespace CNC_CAD.Shapes
{
    public abstract class Shape
    {
        protected List<WPFShape> WpfShapes = new List<WPFShape>();
        protected Shape(){}
        public abstract List<GCodeCommand> GenerateGCodeCommands();

        public virtual List<WPFShape> GetControlShapes()
        {
            return WpfShapes;
        }

        public abstract void Move(Vector2 delta);
        public abstract void Scale(Vector2 multiplication, Vector2 pivot);
        public abstract void Rotate(float angle, Vector2 pivot);
        
    }
}