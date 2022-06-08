using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;
using CNC_CAD.Shapes;

namespace CNC_CAD.Tools
{
    public abstract class SvgElementParser<T> where T : SvgElement, new()
    {
        public virtual T Create(XmlElement element)
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
            var find = "[+\\-]?(?:0|[1-9]\\d*)(?:\\.\\d+)?(?:[eE][+\\-]?\\d+)?";
            var tokens = Regex.Matches(command.Substring(1, command.Length - 1), find)
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