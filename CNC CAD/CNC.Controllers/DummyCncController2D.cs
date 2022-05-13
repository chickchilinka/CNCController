using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CNC_CAD.GCode;
using CNC_CAD.Tools;

namespace CNC_CAD.CNC.Controllers
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
                    Thread.Sleep(300);
                    Console.WriteLine(subCommand);
                }
                _logger.Log("End of commands execution");
            });
            thread.Start(); 
        }
    }
}