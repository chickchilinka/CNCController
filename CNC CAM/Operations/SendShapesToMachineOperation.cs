using System.Collections.Generic;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;
using CNC_CAM.UI.Windows;
using CNC_CAM.Workspaces;
using CNC_CAM.Workspaces.Hierarchy;
using DryIoc;

namespace CNC_CAM.Operations
{
    public class SendShapesToMachineOperation:Operation, IInterruptable
    {
        private Logger _logger;
        private IContainer _container;
        private AbstractController2D _machineController;
        private CurrentConfiguration _config;
        private WorkspaceElementStorage _workspaceElementStorage;
        public SendShapesToMachineOperation(IContainer container, AbstractController2D controller2D, WorkspaceFacade workspaceFacade, CurrentConfiguration config) : base("Send to machine")
        {
            _workspaceElementStorage = container.Resolve<WorkspaceElementStorage>();
            _logger = Logger.CreateFor(this);
            _container = container;
            _machineController = controller2D;
            _config = config;
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
            window.Show();
           
        }

        public void Stop()
        {
            _machineController.Stop();
        }

        private List<Shape> GetOptimalSequence()
        {
            var children = new List<Shape>();   
            foreach (var element in _workspaceElementStorage)
            {
                if (element is WorkspaceElement<SvgRoot> svgElement)
                {
                    children.AddRange(GetAllChildShapes(svgElement.Element));
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
        

        public override void Undo()
        {
            
        }
    }
}