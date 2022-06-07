
using System.Collections.Generic;
using System.Windows;
using CNC_CAD.Configs;

namespace CNC_CAD.GCode
{
    public class GCodeAbsoluteBuilder2D : GCodeBuilder2D<GCodeAbsoluteBuilder2D>
    {
        private readonly Vector _position;
        private bool _fastTravel = true;

        public GCodeAbsoluteBuilder2D(CncConfig config, Vector positionToMove) : base(config)
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
            var physical = Config.ConvertVectorToPhysical(_position);
            if (_fastTravel)
                return new List<string>
                {
                    $"G00 {Config.AxisX}{physical.X.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"{Config.AxisY}{physical.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
                };
            return new List<string>
                {
                    $"G01 {Config.AxisX}{physical.X.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"{Config.AxisY}{physical.Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"F{Config.BaseFeedRate.ToString(System.Globalization.CultureInfo.InvariantCulture)}"
                };
        }
    }
}