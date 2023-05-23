using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;
using CNC_CAM.Tools;

namespace CNC_CAM.SVG.Parsers;

public static class TransfomationMatrixParser
{
    public static Matrix GetTransformationMatrix(this XmlElement element)
    {
        var transformCommand = element.GetAttribute("transform");
        if (string.IsNullOrEmpty(transformCommand))
            return Matrix.Identity;
        var removePart = "^\\w+\\s*";
        var command = Regex.Match(transformCommand, removePart).Value;
        var args = GetCommandArguments(Regex.Replace(transformCommand, removePart, ""));
        switch (command)
        {
            case "matrix":
                return MatrixFromArgs(args);
            case "translate":
                return GetForTranslateCommand(args);
            case "scale":
                return GetForScaleCommand(args);
            case "rotate":
                return GetForRotateCommand(args);
            default:
                return Matrix.Identity;
        }
    }

    public static Matrix GetForTranslateCommand(double[] args)
    {
        var matrix = new Matrix();
        var x = args[0];
        var y = 0d;
        if (args.Length >= 2)
            y = args[1];
        matrix.Translate(x, y);
        return matrix;
    }
    
    public static Matrix GetForScaleCommand(double[] args)
    {
        var matrix = new Matrix();
        var x = args[0];
        var y = x;
        if (args.Length >= 2)
            y = args[1];
        matrix.Scale(x, y);
        return matrix;
    }

    public static Matrix GetForRotateCommand(double[] args)
    {
        var matrix = new Matrix();
        var a = args[0];
        var x = 0d;
        var y = 0d;
        if (args.Length >= 2)
            x = args[1];
        if (args.Length >= 3)
            y = args[2];
        if(x == 0 && y == 0)
            matrix.Rotate(a);
        else 
            matrix.RotateAt(a, x, y);
        return matrix;
    }
    public static Matrix MatrixFromArgs(double[] args)
    {
        return new Matrix(args[0], args[1], args[2], args[3], args[4], args[5]);
    }

    public static double[] GetCommandArguments(this string command)
    {
        var tokens = Regex.Matches(command, RegexPatterns.DecimalsSearchMatcher)
            .Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
        var args = new double[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
            args[i] = double.Parse(tokens[i].Value, new CultureInfo("en-EN"));
        return args;
    }
}