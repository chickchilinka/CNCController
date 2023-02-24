using System;
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

        public static readonly AccuracySettings DefaultAccuracy = new AccuracySettings(1);
        public static readonly CncConfig DefaultCncConfig = new CncConfig()
        {
            HeadDown = -105,
            COMPort = "COM5",
            BaudRate = 115200,
            AccuracySettings = DefaultAccuracy
        };
    }

    public static class Paths
    {
        public static readonly string DocumentsPath = @"%USERPROFILE%\Documents\CNC_CAM\";
        public static readonly string LastSessionFolder = @"Last Session\";
        public static readonly string CncConfigFileName = "CncConfig";
        public static string CncConfigFileFullPath => DocumentsPath + LastSessionFolder + CncConfigFileName;
    }
}