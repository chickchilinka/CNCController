using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Tools;
using Microsoft.Win32;

namespace CNC_CAM.Machine.Controllers
{
    public class DummyCncController2D : AbstractController2D
    {
        private Logger _logger = Logger.CreateForClass(typeof(DummyCncController2D));

        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var command in commands)
            {
                foreach (var subcommand in command)
                {
                    builder.AppendLine(subcommand);
                }
            }

            var dialog = new SaveFileDialog()
            {
                InitialDirectory = "c:\\",
                Filter = "Файл формата GCODE (*.gcode)|*.gcode|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            var shown = dialog.ShowDialog();
            if(!shown ?? false)
                return;
            File.WriteAllText(dialog.FileName, builder.ToString());
        }
        public override void Stop()
        {
        }
    }
}