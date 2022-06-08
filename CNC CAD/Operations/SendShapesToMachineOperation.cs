using System.Collections.Generic;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.GCode;
using CNC_CAD.Tools;
using CNC_CAD.Workspaces;
using CNC_CAD.Shapes;
using CNC_CAD.Windows;

namespace CNC_CAD.Operations
{
    public class SendShapesToMachineOperation:Operation, IInterruptable
    {
        private Logger _logger;
        private AbstractController2D _machineController;
        private Workspace _workspace;
        private CncConfig _config;
        public SendShapesToMachineOperation(AbstractController2D controller2D, Workspace workspace, CncConfig config ) : base("Send to machine")
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
                if (shape is PathShape pathShape)
                {
                    curves.AddRange(pathShape.Curves);
                }
            }
            new DrawGCodeWindow().Draw(curves, _config.AccuracySettings);
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