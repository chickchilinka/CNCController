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
    public class SvgPathDataParser:SvgElementParser<PathShape>
    {
        private Vector _startPoint;
        private Vector _currentPoint;
        private bool returnedToStart = false;
        private List<ICurve> _curves;
        private string lastCommand;
        private Vector _lastSubpath;
        public override PathShape Create(XmlElement element)
        {
            _curves = new List<ICurve>();
            var data = element.GetAttribute("d");
            var id = element.GetAttribute("id");
            var separators = @"(?=[MZLHVCSQTAmzlhvcsqta])";
            var tokens = Regex.Split(data, separators).Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = GetCommandArguments(tokens[0]);
            _startPoint = new Vector(args[0], args[1]);
            _currentPoint = _startPoint;
            _lastSubpath = _currentPoint;
            if (args.Length>2 && args.Length % 2 == 0)
            {
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                _curves.Add(new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both, tokens[0][0]=='m'));
                _currentPoint = _curves[^1].EndPoint;
                lastCommand = tokens[0];
            }
            

            for (var i = 1; i < tokens.Length; i++)
            {
                var curve = GetCurveForCommand(tokens[i]);
                if (curve != null)
                    _curves.Add(curve);
                lastCommand = tokens[i];
            }

            return new PathShape(data, id, _curves, returnedToStart ? _startPoint : null)
            {
                TransformationMatrix = GetTransformationMatrixFromXml(element)
            };
        }

        private ICurve GetCurveForCommand(string command)
        {
            var args = GetCommandArguments(command);
            ICurve curve = null;
            switch (command[0])
            {
                case 'M':
                    curve = MoveToAbsolute(command);
                    break;
                case 'm':
                    curve = MoveToRelative(command);
                    break;
                case 'L':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Both);
                    break;
                case 'l':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Both, true);
                    break;
                case 'h':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Horizontal, true);
                    break;
                case 'H':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Horizontal);
                    break;
                case 'v':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Vertical, true);
                    break;
                case 'V':
                    curve = new SvgLine(args, _currentPoint, SvgLine.Direction.Vertical);
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
                case 'Z':
                case 'z':
                    if (lastCommand[0] == 'm' || lastCommand[0] == 'M')
                    {
                        curve = new SvgLine(new double[]{_lastSubpath.X,_lastSubpath.Y}, _currentPoint,
                            SvgLine.Direction.Both);
                    }
                    else
                    {
                        _currentPoint = _lastSubpath;
                    }
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

        private ICurve MoveToAbsolute(string command)
        {
            var args = GetCommandArguments(command);
            if (args.Length == 2)
            {
                _currentPoint.X = args[0];
                _currentPoint.Y = args[1];
            }
            else if (args.Length % 2 == 0)
            {
                _currentPoint.X = args[0];
                _currentPoint.Y = args[1];
                _lastSubpath = _currentPoint;
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                return new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both);
            }
            _lastSubpath = _currentPoint;
            return null;
        }

        private ICurve MoveToRelative(string command)
        {
            var args = GetCommandArguments(command);
            if (args.Length == 2)
            {
                _currentPoint.X += args[0];
                _currentPoint.Y += args[1];
            }
            else if (args.Length % 2 == 0)
            {
                _currentPoint.X += args[0];
                _currentPoint.Y += args[1];
                _lastSubpath = _currentPoint;
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                return new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both, true);
            }
            _lastSubpath = _currentPoint;
            return null;
        }
    }
}