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

namespace CNC_CAM.Operations
{
    public class SendShapesToMachineOperation:Operation, IInterruptable
    {
        private Logger _logger;
        private AbstractController2D _machineController;
        private Workspace _workspace;
        private CurrentConfiguration _config;
        public SendShapesToMachineOperation(AbstractController2D controller2D, Workspace workspace, CurrentConfiguration config) : base("Send to machine")
        {
            _logger = Logger.CreateFor(this);
            _workspace = workspace;
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

        public List<Shape> GetOptimalSequence()
        {
            if (_workspace.Shapes == null || _workspace.Shapes.Count == 0)
                return new List<Shape>();
            return new OptimalPathBuilder<Shape>().GetPathForTransforms(_workspace.GetAllChildShapes(), out double sum);
        }

        public override void Undo()
        {
            
        }
    }
}