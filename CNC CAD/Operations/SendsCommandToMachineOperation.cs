using System.Collections.Generic;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Tools;
using CNC_CAD.Workspaces;
using CNC_CAD.Shapes;

namespace CNC_CAD.Operations
{
    public class SendsCommandToMachineOperation:Operation
    {
        private Logger _logger;
        private AbstractController2D _machineController;
        private Workspace _workspace;
        private CncConfig _config;
        public SendsCommandToMachineOperation(AbstractController2D controller2D, Workspace workspace, CncConfig config ) : base("Send to machine")
        {
            _logger = Logger.CreateFor(this);
            _workspace = workspace;
            _machineController = controller2D;
            _config = config;
        }

        public override void Execute()
        {
            foreach (var shape in GetOptimalSequence())
            {
                _machineController.ExecuteGCodeCommands(shape.GenerateGCodeCommands(_config));
            }
        }

        public List<Shape> GetOptimalSequence()
        {
            return _workspace.Shapes;
        }

        public override void Undo()
        {
            
        }
    }
}