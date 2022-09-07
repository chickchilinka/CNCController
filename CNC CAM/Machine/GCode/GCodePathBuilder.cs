using System.Collections.Generic;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.Machine.GCode
{
    public class GCodePathBuilder : GCodeBuilder2D<GCodePathBuilder>
    {
        private SvgPath _svgPath;
        private CncConfig _config;
        public GCodePathBuilder(CncConfig config, SvgPath svgPath) : base(config)
        {
            _config = config;
            _svgPath = svgPath;
        }

        protected override List<string> GenerateCommands()
        {
            List<string> commands = new();
            ICurve lastCurve = null;
            commands.AddRange(WithAbsoluteMove(_config, _svgPath.ToGlobalPoint(_svgPath.StartPoint))
                .SetHeadDownAtStart(false)
                .SetHeadDownAtEnd(true)
                .Build());
            foreach (var curve in _svgPath.Curves)
            {
                if (lastCurve != null && lastCurve.EndPoint != curve.StartPoint)
                {
                    commands.AddRange(WithAbsoluteMove(_config, curve.ToGlobalPoint(curve.StartPoint))
                        .SetHeadDownAtStart(false)
                        .SetHeadDownAtEnd(true)
                        .SetFastTravel(true)
                        .Build());
                }
                foreach (var point in curve.Linearize(_config.AccuracySettings))
                {
                    commands.AddRange(WithAbsoluteMove(_config, point)
                        .SetFastTravel(false)
                        .Build());   
                }
                lastCurve = curve;
            }
            return commands;
        }
    }
}