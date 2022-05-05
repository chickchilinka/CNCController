using System;
using System.Collections.Generic;
using System.Linq;
using CNC_CAD.GCode;

namespace CNC_CAD.CNC.Controllers
{
    public class DummyCncController2D:AbstractController2D
    {
        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            Console.WriteLine("Executing:");
            foreach (var subCommand in commands.SelectMany(command=>command))
            {
                Console.WriteLine(subCommand);   
            }
            Console.WriteLine("End of commands execution");
        }
    }
}