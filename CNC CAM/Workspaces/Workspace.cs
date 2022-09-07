using System.Collections.Generic;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.UI.CustomWPFElements;

namespace CNC_CAM.Workspaces
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

        public List<Shape> GetAllChildShapes()
        {
            var children = new List<Shape>();
            if (Shapes.Count > 0)
            {
                foreach (var shape in Shapes)
                {
                    children.AddRange(GetAllChildShapes(shape));
                }
            }

            return children;
        }
        public List<Shape> GetAllChildShapes(Shape element)
        {
            var list = new List<Shape>();
            if (element is SvgGroupElement group)
            {
                foreach (var child in group.Children)
                {
                    list.AddRange(GetAllChildShapes(child));
                }
                return list;
            }
            list.Add(element);
            return list;
        }
    }
}