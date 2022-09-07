using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;

namespace CNC_CAM.SVG.Parsers
{
    public abstract class SvgElementParser
    {
        public abstract SvgElement Create(XmlElement element);
    }
    public abstract class SvgElementParser<T>:SvgElementParser where T : SvgElement, new()
    {
        public override T Create(XmlElement element)
        {
            return new T()
            {
                Name = GetId(element),
                TransformationMatrix = GetTransformationMatrixFromXml(element)
            };
        }

        protected Matrix GetTransformationMatrixFromXml(XmlElement element)
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
                default:
                    return Matrix.Identity;
            }
        }

        protected Matrix MatrixFromArgs(double[] args)
        {
            return new Matrix(args[0], args[1], args[2], args[3], args[4], args[5]);
        }

        public static double[] GetCommandArguments(string command)
        {
            var tokens = Regex.Matches(command, Const.RegexPatterns.DecimalsSearchMatcher)
                .Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
            var args = new double[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
                args[i] = double.Parse(tokens[i].Value, new CultureInfo("en-EN"));
            return args;
        }

        protected string GetId(XmlElement element)
        {
            return element.GetAttribute("id");
        } 
    }
}