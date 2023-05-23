using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using CNC_CAM.Base;
using CNC_CAM.Configuration;
using CNC_CAM.Machine.GCode;
using CNC_CAM.SVG.Parsers;
using CNC_CAM.Tools;
using Vector = System.Windows.Vector;

namespace CNC_CAM.Machine.Controllers
{
    public class NotCompleteMessageException : Exception
    {
        public NotCompleteMessageException(string message) : base(message){}
    }
    public class DrawingHeadController : AbstractController2D
    {
        private Logger _logger = Logger.CreateForClass(typeof(DummyCncController2D));
        private CurrentConfiguration _config;
        private bool Interrupt { get; set; } = false;

        public DrawingHeadController(CurrentConfiguration config)
        {
            _config = config;
        }

        public override void ExecuteGCodeCommands(IEnumerable<GCodeCommand> commands)
        {
            var controller = SimpleSerialController.CreateSerialController(_config);
            _logger.Log("Executing:");
            Thread thread = new Thread(() =>
            {
                foreach (var subCommand in commands.SelectMany(command => command))
                {
                    if (Interrupt)
                    {
                        Interrupt = false;
                        return;
                    }

                    _logger.Log(subCommand);
                    controller.SendString(subCommand);
                    controller.Read();
                    WaitUntilIsCommandDone(subCommand, controller);
                }

                _logger.Log("End of commands execution");
                controller.Dispose();
            });
            thread.Start();
        }

        //MPos:0.000,0.000,0.000
        public Vector3 GetCurrentPosition(string data)
        {
            string[] substrings = data.Split('|');
            if (substrings.Length < 2)
                throw new NotCompleteMessageException("No response from cnc machine");
            double[] args = substrings[1].GetCommandArguments();
            return new((float) args[0], (float) args[1], (float) args[2]);
        }

        private void WaitUntilIsCommandDone(string command, SimpleSerialController controller)
        {
            while (true)
            {
                if (Interrupt)
                {
                    return;
                }

                if (command.StartsWith("G01") || command.StartsWith("G00"))
                {
                    var commandPosArgs = command.Remove(0, 3).GetCommandArguments();
                    try
                    {
                        if (commandPosArgs.Length == 0)
                            return;
                        if (commandPosArgs.Length == 1 && command.ToLower().Contains("z"))
                        {
                            if (CheckZPosition(commandPosArgs[0], controller))
                                return;
                        }

                        if (commandPosArgs.Length >= 2 && command.ToLower().Contains("y") &&
                            command.ToLower().Contains("x"))
                        {
                            if (CheckXYPosition(new Vector(commandPosArgs[0], commandPosArgs[1]), controller))
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(ex.StackTrace);
                        Thread.Sleep(50);
                        continue;
                    }
                }
                else
                {
                    return;
                }

                Thread.Sleep(50);
            }
        }

        private bool CheckZPosition(double zPos, SimpleSerialController controller)
        {
            controller.SendString("?");
            var read = ExtractLastStatus(controller.Read());
            if (read == null)
                return false;
            if (read.StartsWith("<"))
            {
                var pos = GetCurrentPosition(read);
                if (Math.Abs(zPos - pos.Z) < 0.001d)
                    return true;
            }

            return false;
        }

        private bool CheckXYPosition(Vector position, SimpleSerialController controller)
        {
            controller.SendString("?");
            var read = ExtractLastStatus(controller.Read());
            if (read == null)
                return false;
            var curPosition = GetCurrentPosition(read);
            var v2CurPosition = new Vector(curPosition.X, curPosition.Y);
            if ((v2CurPosition - position).Length < 0.1d)
                return true;
            return false;
        }

        private string ExtractLastStatus(string read)
        {
            var search = "^<[^\\s]*";
            var matches = Regex.Matches(read, search);
            if (matches.Count == 0)
                return null;
            return matches[^1].Value;
        }

        public override void Stop()
        {
            Interrupt = true;
        }
    }
}