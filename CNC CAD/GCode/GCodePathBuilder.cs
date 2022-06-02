using System.Collections.Generic;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.Shapes;

namespace CNC_CAD.GCode
{
    public class GCodePathBuilder : GCodeBuilder2D
    {
        private PathShape _pathShape;
        private CncConfig _config;
        public GCodePathBuilder(CncConfig config, PathShape path) : base(config)
        {
            _config = config;
            _pathShape = path;
        }

        protected override List<string> GenerateCommands()
        {
            List<string> commands = new();
            ICurve lastCurve = null;
            foreach (var curve in _pathShape.Curves)
            {
                if (lastCurve != null && lastCurve.EndPoint != curve.StartPoint)
                {
                    commands.AddRange(WithAbsoluteMove(_config, curve.StartPoint)
                        .SetHeadDownAtStart(false)
                        .SetHeadDownAtEnd(true)
                        .Build());
                }
                foreach (var point in curve.Linearize(_config.AccuracySettings))
                {
                    commands.AddRange(WithAbsoluteMove(_config, point).Build());   
                }
                lastCurve = curve;
            }
            return commands;
        }
    }
}