using System.Collections.Generic;
using CNC_CAD.GCode;

namespace CNC_CAD.CNC.Controllers
{
    public abstract class AbstractController2D
    {
        public abstract void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands);
        public abstract void Stop();
    }
}