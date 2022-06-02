using System;
using System.Collections.Generic;
using CNC_CAD.CNC.Controllers;
using System.Windows;
using CNC_CAD.Configs;

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
            return new List<string>{$"G00 {Config.AxisX}{physical.X.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                                    $"{Config.AxisY}{physical.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}"};
        }
    }
}