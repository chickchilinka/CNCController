using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.Machine.GCode
{
    public class GCodeAbsoluteBuilder2D : GCodeBuilder2D<GCodeAbsoluteBuilder2D>
    {
        private readonly Vector _position;
        private bool _fastTravel = true;

        public GCodeAbsoluteBuilder2D(CurrentConfiguration settings, Vector positionToMove) : base(settings)
        {
            _position = positionToMove;
        }

        public GCodeAbsoluteBuilder2D SetFastTravel(bool fast)
        {
            _fastTravel = fast;
            return this;
        }

        protected override List<string> GenerateCommands()
        {
            var physical = CurrentConfiguration.ConvertVectorToPhysical(_position);
            if (_fastTravel)
                return new List<string>
                {
                    $"G00 {ControlSettings.AxisX}{physical.X.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"{ControlSettings.AxisY}{physical.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
                };
            return new List<string>
                {
                    $"G01 {ControlSettings.AxisX}{physical.X.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"{ControlSettings.AxisY}{physical.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"F{ControlSettings.BaseFeedRate.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
                };
        }
    }
}