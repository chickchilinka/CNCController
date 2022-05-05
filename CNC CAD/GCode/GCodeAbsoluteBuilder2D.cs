using System.Collections.Generic;
using System.Numerics;
using CNC_CAD.CNC.Controllers;

namespace CNC_CAD.GCode
{
    public class GCodeAbsoluteBuilder2D:GCodeBuilder2D
    {
        private readonly Vector2 _position;
        public GCodeAbsoluteBuilder2D(CNCConfig config, Vector2 positionToMove):base(config)
        {
            _position = positionToMove;
        }

        public override GCodeCommand Build()
        {
            var commandsSequence = new List<string>();
            if (HeadPositionAtStart != null)
            {
                commandsSequence.Add($"G0 {Config.AxisZ}{HeadPositionAtStart}");
            }
            commandsSequence.Add($"G0 {Config.AxisX}{_position.X}{Config.AxisY}{_position.Y}");
            if (HeadPositionAtEnd != null)
            {
                commandsSequence.Add($"G0 {Config.AxisZ}{HeadPositionAtEnd}");
            }

            return new GCodeCommand(commandsSequence.ToArray());
        }
    }
}