using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Tools;

namespace CNC_CAM.Machine.CNC.Controllers
{
    public class DummyCncController2D : AbstractController2D
    {
        private Logger _logger = Logger.CreateForClass(typeof(DummyCncController2D));

        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            Thread thread = new Thread(() =>
            {
                _logger.Log("Executing:");
                foreach (var subCommand in commands.SelectMany(command => command))
                {
                    Thread.Sleep(10);
                    Console.WriteLine(subCommand);
                }
                _logger.Log("End of commands execution");
            });
            thread.Start(); 
        }
        public override void Stop()
        {
        }
    }
}