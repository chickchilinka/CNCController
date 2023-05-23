using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.SVG.Subpaths;

namespace CNC_CAM.SVG.Parsers
{
    public class SvgPathDataParser:SvgElementParser<SvgPath>
    {
        protected List<ICurve> _curves;
        private Vector _startPoint;
        private Vector _currentPoint;
        private bool returnedToStart = false;
        private string lastCommand;
        private Vector _lastSubpathEnd;
        private SvgCubicBezier _lastCubicBezier;
        public override SvgPath Create(XmlElement element)
        {
            _curves = new List<ICurve>();
            var data = element.GetAttribute("d");
            var id = element.GetAttribute("id"); ;
            _curves = GetCurves(data);
            return new SvgPath(data, id, _curves, returnedToStart ? _startPoint : null)
            {
                TransformationMatrix = element.GetTransformationMatrix()
            };
        }

        protected List<ICurve> GetCurves(string data)
        {
            var curves = new List<ICurve>();
            var separators = @"(?=[MZLHVCSQTAmzlhvcsqta])";
            var tokens = Regex.Split(data, separators).Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = tokens[0].GetCommandArguments();
            _startPoint = new Vector(args[0], args[1]);
            _currentPoint = _startPoint;
            _lastSubpathEnd = _currentPoint;
            if (args.Length>2 && args.Length % 2 == 0)
            {
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                curves.Add(new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both, tokens[0][0]=='m'));
                _currentPoint = curves[^1].EndPoint;
                lastCommand = tokens[0];
            }
            

            for (var i = 1; i < tokens.Length; i++)
            {
                var curve = GetCurveForCommand(tokens[i]);
                if (curve != null)
                    curves.Add(curve);
                lastCommand = tokens[i];
            }

            return curves;
        }
        private ICurve GetCurveForCommand(string command)
        {
            var args = command.GetCommandArguments();
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
                case 'Q':
                    curve = new SvgQuadraticBezier(args, _currentPoint);
                    break;
                case 'q':
                    curve = new SvgQuadraticBezier(args, _currentPoint, true);
                    break;
                case 'S':
                    curve = new SvgCubicBezierShort(args, _lastCubicBezier, _currentPoint);
                    break;
                case 's':
                    curve = new SvgCubicBezierShort(args, _lastCubicBezier, _currentPoint, true);
                    break;
                case 'Z':
                case 'z':
                    curve = new SvgLine(new double[]{_lastSubpathEnd.X,_lastSubpathEnd.Y}, _currentPoint,
                            SvgLine.Direction.Both);
                    break;
            }

            if (curve != null)
            {
                _currentPoint = curve.EndPoint;
            }
            
            _lastCubicBezier = curve as SvgCubicBezier;
            
            return curve;
        }

        private ICurve MoveToAbsolute(string command)
        {
            var args = command.GetCommandArguments();
            if (args.Length == 2)
            {
                _currentPoint.X = args[0];
                _currentPoint.Y = args[1];
            }
            else if (args.Length % 2 == 0)
            {
                _currentPoint.X = args[0];
                _currentPoint.Y = args[1];
                _lastSubpathEnd = _currentPoint;
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                return new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both);
            }
            _lastSubpathEnd = _currentPoint;
            return null;
        }

        private ICurve MoveToRelative(string command)
        {
            var args = command.GetCommandArguments();
            if (args.Length == 2)
            {
                _currentPoint.X += args[0];
                _currentPoint.Y += args[1];
            }
            else if (args.Length % 2 == 0)
            {
                _currentPoint.X += args[0];
                _currentPoint.Y += args[1];
                _lastSubpathEnd = _currentPoint;
                double[] lineArgs = new double[args.Length - 2];
                Array.Copy(args, 2, lineArgs, 0, args.Length - 2);
                return new SvgLine(lineArgs, _currentPoint, SvgLine.Direction.Both, true);
            }
            _lastSubpathEnd = _currentPoint;
            return null;
        }
    }
}