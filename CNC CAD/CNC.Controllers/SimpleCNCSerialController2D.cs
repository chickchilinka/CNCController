using System.Collections.Generic;
using System.Linq;
using CNC_CAD.Base;
using CNC_CAD.GCode;

namespace CNC_CAD.CNC.Controllers
{
    public class SimpleCncSerialController2D:AbstractController2D
    {
        private CNCConfig _config;
        public SimpleCncSerialController2D(CNCConfig config)
        {
            _config = config;
        }
        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            using (var controller = SimpleSerialController.CreateSerialController(_config))
            {
                foreach (var subCommand in commands.SelectMany(command => command))
                {
                    controller.SendString(subCommand);
                }
            }
        }
    }
}