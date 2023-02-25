using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;
using DryIoc;

namespace CNC_CAM.Machine.GCode
{
    public abstract class GCodeBuilder2D
    {
        public static GCodeAbsoluteBuilder2D WithAbsoluteMove(CurrentConfiguration config, Vector position)
        {
            return new GCodeAbsoluteBuilder2D(config, position);
        }

        public static GCodePathBuilder ForPath(CurrentConfiguration config, SvgPath svgPath)
        {
            return new GCodePathBuilder(config, svgPath);
        }
    }

    public abstract class GCodeBuilder2D<T> : GCodeBuilder2D where T : GCodeBuilder2D<T>
    {
        protected Logger Logger;
        protected bool? HeadDownAtStart;
        protected bool? HeadDownAtEnd;
        protected CurrentConfiguration CurrentConfiguration;
        protected CNCHeadSettings HeadSettings;
        protected CNCControlSettings ControlSettings;

        protected GCodeBuilder2D(CurrentConfiguration currentConfiguration)
        {
            CurrentConfiguration = currentConfiguration;
            HeadSettings = CurrentConfiguration.GetCurrentConfig<CNCHeadSettings>();
            ControlSettings = CurrentConfiguration.GetCurrentConfig<CNCControlSettings>();
            Logger = Logger.CreateForClass(this.GetType());
        }

        public T SetHeadDownAtStart(bool set)
        {
            HeadDownAtStart = set;
            return (T)this;
        }

        public T SetHeadDownAtEnd(bool set)
        {
            HeadDownAtEnd = set;
            return (T)this;
        }

        protected void AddHeadCommandForStart(List<string> commandsSequence)
        {
            if (HeadDownAtStart != null)
            {
                var headConfig = CurrentConfiguration.GetCurrentConfig<CNCHeadSettings>();
                var headPosStart = HeadDownAtStart == true
                    ? headConfig.HeadDown
                    : headConfig.HeadUp;
                commandsSequence.Add($"G00 {ControlSettings.AxisZ}{headPosStart}");
            }
        }

        protected void AddHeadCommandForEnd(List<string> commandsSequence)
        {
            if (HeadDownAtEnd != null)
            {
                var headPosEnd = HeadDownAtEnd == true ? HeadSettings.HeadDown : HeadSettings.HeadUp;
                commandsSequence.Add($"G00 {ControlSettings.AxisZ}{headPosEnd}");
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