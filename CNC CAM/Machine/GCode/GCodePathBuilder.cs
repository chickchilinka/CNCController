using System.Collections.Generic;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.Machine.GCode
{
    public class GCodePathBuilder : GCodeBuilder2D<GCodePathBuilder>
    {
        private SvgPath _svgPath;
        private CurrentConfiguration _currentConfiguration;
        public GCodePathBuilder(CurrentConfiguration currentConfiguration, SvgPath svgPath) : base(currentConfiguration)
        {
            _currentConfiguration = currentConfiguration;
            _svgPath = svgPath;
        }

        protected override List<string> GenerateCommands()
        {
            List<string> commands = new();
            ICurve lastCurve = null;
            commands.AddRange(WithAbsoluteMove(_currentConfiguration, _svgPath.ToGlobalPoint(_svgPath.StartPoint))
                .SetHeadDownAtStart(false)
                .SetHeadDownAtEnd(true)
                .Build());
            foreach (var curve in _svgPath.Curves)
            {
                if (lastCurve != null && lastCurve.EndPoint != curve.StartPoint)
                {
                    commands.AddRange(WithAbsoluteMove(_currentConfiguration, curve.ToGlobalPoint(curve.StartPoint))
                        .SetHeadDownAtStart(false)
                        .SetHeadDownAtEnd(true)
                        .SetFastTravel(true)
                        .Build());
                }
                foreach (var point in curve.Linearize(_currentConfiguration.GetCurrentConfig<AccuracySettings>()))
                {
                    commands.AddRange(WithAbsoluteMove(_currentConfiguration, point)
                        .SetFastTravel(false)
                        .Build());   
                }
                lastCurve = curve;
            }
            return commands;
        }
    }
}