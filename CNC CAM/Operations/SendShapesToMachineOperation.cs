using System.Collections.Generic;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.CNC.Controllers;
using CNC_CAM.Machine.Configs;
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
        private AbstractController2D _machineController;
        private WorkspaceFacade _workspaceFacade;
        private CurrentConfiguration _config;
        private WorkspaceElementStorage _workspaceElementStorage;
        public SendShapesToMachineOperation(IContainer container, AbstractController2D controller2D, WorkspaceFacade workspaceFacade, CurrentConfiguration config) : base("Send to machine")
        {
            _workspaceElementStorage = container.Resolve<WorkspaceElementStorage>();
            _logger = Logger.CreateFor(this);
            _workspaceFacade = workspaceFacade;
            _machineController = controller2D;
            _config = config;
        }

        public override void Execute()
        {
            var gcodes = new List<GCodeCommand>();
            gcodes.Add(new GCodeCommand(new List<string>{"G90", "G28"}));
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
            new DrawGCodeWindow().Draw(curves, _config.GetCurrentConfig<AccuracySettings>());
            foreach (var shape in sequence)
            {
                gcodes.AddRange(shape.GenerateGCodeCommands(_config));
            }
            gcodes.Add(new GCodeCommand(new List<string>{"G90", "G28"}));
            _machineController.ExecuteGCodeCommands(gcodes);
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