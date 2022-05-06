using System.Collections.Generic;
using CNC_CAD.CustomWPFElements;
using CNC_CAD.Shapes;

namespace CNC_CAD.Workspaces
{
    public class Workspace
    {
        private List<Shape> _shapes;
        public Workspace2D Workspace2D { get; private set; }

        public Workspace()
        {
            _shapes = new List<Shape>();
            Workspace2D = new Workspace2D();
        }

        public void AddShape(Shape shape)
        {
            foreach (var wpfShape in shape.GetControlShapes())
            {
                Workspace2D.AddShape(wpfShape);    
            }
        }
    }
}