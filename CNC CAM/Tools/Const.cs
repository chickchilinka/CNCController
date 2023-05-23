using System;
using System.Collections.Generic;
using CNC_CAM.Configuration.Data;

namespace CNC_CAM.Tools;

public static class RegexPatterns
{
    public static readonly string DecimalsSearchMatcher = "[+\\-]?(?:0|[1-9]\\d*)?(?:\\.\\d+)?(?:[eE][+\\-]?\\d+)?";
    public static readonly string DecimalMatcher = "^([-+]?[0-9]+[.]?[0-9]*)$";
    public static readonly string DecimalInputMatcher = "^([-+]?[0-9]*[.,]?[0-9]*)$";
    public static readonly string IntInputMatcher = "^([-+]?[0-9]*)$";
}
public static class Const
{
    
    public static class Formatters
    {
        public static readonly string MousePositionFormatMM = "Положение курсора X:{0}mm Y:{1}mm";
    }

    public static class Configs
    {
        public static readonly IReadOnlyList<BaseConfig> DefaultConfigs = new List<BaseConfig>()
        {
            BaseConfig.Create<UserSettings>("default"),
            BaseConfig.Create<MachineConfig>("default"),
            BaseConfig.Create<DrawingHeadConfig>("default"),
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