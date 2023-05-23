using System.Collections.Generic;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;
using CNC_CAM.UI.Windows;
using CNC_CAM.Workspaces;
using CNC_CAM.Workspaces.Hierarchy;
using DryIoc;

namespace CNC_CAM.Operations
{
    public class DrawOperation:Operation, IInterruptable
    {
        private IContainer _container;
        private WorkspaceFacade _workspaceFacade;
        public DrawOperation(IContainer container) : base("Send to machine")
        {
            _workspaceFacade = container.Resolve<WorkspaceFacade>();
            _container = container;
        }

        public override void Execute()
        {
            var sequence = GetOptimalSequence();
            List<ICurve> curves = new List<ICurve>();
            foreach (var shape in sequence)
            {
                if (shape is SvgPath pathShape)
                {
                    curves.AddRange(pathShape.Curves);
                }
                else if (shape is ICurve)
                {
                    curves.Add((ICurve)shape);
                }
            }
            var window = _container.Resolve<ExportWindow>();
            window.Initialize(sequence, curves);
            window.ShowDialog();
        }

        public void Stop()
        {
        }

        private List<Shape> GetOptimalSequence()
        {
            var children = new List<Shape>();   
            foreach (var element in _workspaceFacade.GetElements())
            {
                if (element is WorkspaceElement<SvgRoot> svgElement)
                {
                    children.AddRange(GetAllChildShapes(svgElement.TransformElement));
                }
            }
            return new OptimalPathBuilder<Shape>().GetPathForTransforms(children, out double sum);
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