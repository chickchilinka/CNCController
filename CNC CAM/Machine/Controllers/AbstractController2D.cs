using System.Collections.Generic;
using CNC_CAM.Machine.GCode;

namespace CNC_CAM.Machine.Controllers
{
    public abstract class AbstractController2D
    {
        public abstract void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands);
        public abstract void Stop();
    }
}