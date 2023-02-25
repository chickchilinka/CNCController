using System;
using System.Collections.Generic;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;

namespace CNC_CAM.Tools;

public static class Const
{
    public static class RegexPatterns
    {
        public static readonly string DecimalsSearchMatcher = "[+\\-]?(?:0|[1-9]\\d*)?(?:\\.\\d+)?(?:[eE][+\\-]?\\d+)?";
        public static readonly string DecimalMatcher = "^([-+]?[0-9]+[.]?[0-9]*)$";
    }

    public static class Formatters
    {
        public static readonly string MousePositionFormatMM = "Cursor position X:{0}mm Y:{1}mm";
    }

    public static class Configs
    {
        public static readonly IReadOnlyList<BaseConfig> DefaultConfigs = new List<BaseConfig>()
        {
            BaseConfig.Create<AccuracySettings>("default"),
            BaseConfig.Create<CNCConnectionSettings>("default"),
            BaseConfig.Create<CNCControlSettings>("default"),
            BaseConfig.Create<CNCHeadSettings>("default"),
            BaseConfig.Create<WorksheetConfig>("default")
        };
    }

    public static class Paths
    {
        public const string DocumentsPath = @"%USERPROFILE%\Documents\CNC_CAM\";
        public const string ConfigurationsPath = DocumentsPath + @"Configs\";
        public const string LastConfigsFilename = @"LastSession";
    }
}