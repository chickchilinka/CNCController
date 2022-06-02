using System.Collections.Generic;
using System.Windows;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using CNC_CAD.Shapes;
using CNC_CAD.Tools;

namespace CNC_CAD.GCode
{
    public abstract class GCodeBuilder2D
    {
        protected Logger Logger;
        protected bool? HeadDownAtStart;
        protected bool? HeadDownAtEnd;
        protected CncConfig Config;

        protected GCodeBuilder2D(CncConfig config)
        {
            Config = config;
            Logger = Logger.CreateForClass(this.GetType());
        }
        
        public static GCodeBuilder2D WithAbsoluteMove(CncConfig config, Vector position)
        {
            return new GCodeAbsoluteBuilder2D(config, position);
        }
        
        public static GCodeBuilder2D ForPath(CncConfig config, PathShape path)
        {
            return new GCodePathBuilder(config, path);
        }

        public GCodeBuilder2D SetHeadDownAtStart(bool set)
        {
            HeadDownAtStart = set;
            return this;
        }
        public GCodeBuilder2D SetHeadDownAtEnd(bool set)
        {
            HeadDownAtEnd = set;
            return this;
        }

        protected void AddHeadCommandForStart(List<string> commandsSequence)
        {
            if (HeadDownAtStart != null)
            {
                var headPosStart = HeadDownAtStart == true ? Config.HeadDown : Config.HeadUp;
                commandsSequence.Add($"G00 {Config.AxisZ}{headPosStart}");
            }
        }

        protected void AddHeadCommandForEnd(List<string> commandsSequence)
        {
            if (HeadDownAtEnd != null)
            {
                var headPosEnd = HeadDownAtEnd == true ? Config.HeadDown : Config.HeadUp;
                commandsSequence.Add($"G00 {Config.AxisZ}{headPosEnd}");
            }
        }

        public virtual List<string> Build()
        {
            var commandsSequence = new List<string>();
            AddHeadCommandForStart(commandsSequence);
            commandsSequence.AddRange(GenerateCommands());
            AddHeadCommandForEnd(commandsSequence);
            return commandsSequence;
        }

        protected abstract List<string> GenerateCommands();
    }
}