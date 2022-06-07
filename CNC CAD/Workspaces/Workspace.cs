using System.Collections.Generic;
using CNC_CAD.CustomWPFElements;
using CNC_CAD.Shapes;

namespace CNC_CAD.Workspaces
{
    public class Workspace
    {
        public List<Shape> Shapes { get; }
        public Workspace2D Workspace2D { get; }

        public Workspace()
        {
            Shapes = new List<Shape>();
            Workspace2D = new Workspace2D();
        }

        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
            foreach (var wpfShape in shape.GetControlShapes())
            {
                Workspace2D.AddShape(wpfShape);    
            }
        }
        
        public void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
            foreach (var wpfShape in shape.GetControlShapes())
            {
                Workspace2D.RemoveShape(wpfShape);    
            }
        }
    }
}