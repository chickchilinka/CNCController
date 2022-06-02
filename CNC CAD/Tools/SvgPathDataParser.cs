using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using CNC_CAD.Curves;
using CNC_CAD.Shapes;

namespace CNC_CAD.Tools
{
    public class SvgPathDataParser
    {
        private  Vector _startPoint;
        private  Vector _currentPoint;

        public  PathShape CreatePath(XmlElement element, PathShape last = null)
        {
            var curves = new List<ICurve>();
            var data = element.GetAttribute("d");
            var id = element.GetAttribute("id");
            double strokeWidth = 1;
            double.TryParse(element.GetAttribute("stroke-width"), out strokeWidth);
            var separators = @"(?=[MZLHVCSQTAmzlhvcsqta])";
            var tokens = Regex.Split(data, separators).Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = GetCommandArguments(tokens[0]);
            _startPoint = new Vector(args[0], args[1]);
            _currentPoint = _startPoint;
            if (tokens[0][0] == 'm' && last!=null)
            {
                _startPoint += last.EndPoint;
                _currentPoint += last.EndPoint;
            }
            for (var i = 1; i < tokens.Length; i++)
            {
                var curve = GetCurveForCommand(tokens[i]);
                if (curve != null)
                    curves.Add(curve);
            }

            return new PathShape(data, id, curves);
        }

        private  ICurve GetCurveForCommand(string command)
        {
            var args = GetCommandArguments(command);
            ICurve curve;
            switch (command[0])
            {
                case 'M':
                    curve = MoveToAbsolute(command);
                    break;
                case 'm':
                    curve = MoveToRelative(command);
                    break;
                case 'L':
                    curve = new SvgLine(args, _currentPoint);
                    break;
                case 'l':
                    curve = new SvgLine(args, _currentPoint, true);
                    break;
                case 'A':
                    curve = new SvgArc(args, _currentPoint);
                    break;
                case 'a':
                    curve = new SvgArc(args, _currentPoint, true);
                    break;
                case 'C':
                    curve = new SvgCubicBezier(args, _currentPoint);
                    break;
                case 'c':
                    curve = new SvgCubicBezier(args, _currentPoint, true);
                    break;
                default:
                    curve = null;
                    break;
            }

            if (curve != null)
            {
                _currentPoint = curve.EndPoint;
            }

            return curve;
        }

        private  ICurve MoveToAbsolute(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X = args[0];
            _currentPoint.Y = args[1];
            return null;
        }

        private  ICurve MoveToRelative(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X += args[0];
            _currentPoint.Y += args[1];
            return null;
        }

        public  double[] GetCommandArguments(string command)
        {
            var separators = "[\\s,]+";
            var tokens = Regex.Split(command.Substring(1, command.Length - 1), separators)
                .Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = new double[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
                args[i] = double.Parse(tokens[i], new CultureInfo("en-EN"));
            return args;
        }
    }
}