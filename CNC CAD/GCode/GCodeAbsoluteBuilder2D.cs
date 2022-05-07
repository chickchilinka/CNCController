using System.Collections.Generic;
using CNC_CAD.CNC.Controllers;
using System.Windows;

namespace CNC_CAD.GCode
{
    /// <summary>
    /// Строитель команд для рисование линий с абсолютными координатами 
    /// </summary>
    public class GCodeAbsoluteBuilder2D : GCodeBuilder2D
    {
        private readonly Vector _position;

        public GCodeAbsoluteBuilder2D(CncConfig config, Vector positionToMove) : base(config)
        {
            _position = positionToMove;
        }
        
        protected override List<string> GenerateCommands()
        {
            var physical = Config.ConvertVectorToPhysical(_position);
            return new List<string>{$"G0 {Config.AxisX}{physical.X}{Config.AxisY}{physical.Y}"};
        }
    }
}