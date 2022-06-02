using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CNC_CAD.Base;
using CNC_CAD.Configs;
using CNC_CAD.GCode;
using CNC_CAD.Tools;

namespace CNC_CAD.CNC.Controllers
{
    public class SimpleCncSerialController2D:AbstractController2D
    {
        private Logger _logger = Logger.CreateForClass(typeof(DummyCncController2D));
        private CncConfig _config;
        public SimpleCncSerialController2D(CncConfig config)
        {
            _config = config;
        }
        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            _logger.Log("Executing:");
            Thread thread = new Thread(() =>
            {
                using (var controller = SimpleSerialController.CreateSerialController(_config))
                {
                    foreach (var subCommand in commands.SelectMany(command => command))
                    {
                        _logger.Log(subCommand);
                        controller.SendString(subCommand);
                        Thread.Sleep(300);
                    }
                }
                _logger.Log("End of commands execution");
            });
            thread.Start();
        }
    }
}