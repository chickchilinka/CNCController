namespace CNC_CAD.Tools;

public static class Const
{
    public static class RegexPatterns
    {
        public static string DecimalsSearchMatcher = "[+\\-]?(?:0|[1-9]\\d*)(?:\\.\\d+)?(?:[eE][+\\-]?\\d+)?";
        public static string DecimalMatcher = "^([-+]?[0-9]+[.]?[0-9]*)$";
    }

    public static class Formatters
    {
        public static string MousePositionFormatMM = "Cursor position X:{0}mm Y:{1}mm";
    }
}