using System.Numerics;
using CNC_CAD.CNC.Controllers;

namespace CNC_CAD.GCode
{
    public abstract class GCodeBuilder2D
    {
        protected float? HeadPositionAtStart;
        protected float? HeadPositionAtEnd;
        protected CNCConfig Config;

        protected GCodeBuilder2D(CNCConfig config)
        {
            Config = config;
        }
        
        public static GCodeBuilder2D WithAbsoluteMove(CNCConfig config, Vector2 position)
        {
            return new GCodeAbsoluteBuilder2D(config, position);
        }

        public void SetHeadPositionAtStart(float position)
        {
            HeadPositionAtStart = position;
        }
        public void SetHeadPositionAtEnd(float position)
        {
            HeadPositionAtEnd = position;
        }

        public abstract GCodeCommand Build();
    }
}