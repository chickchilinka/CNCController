using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CNC_CAD.CNC.Controllers;
using System.Windows;
using CNC_CAD.SvgTools;
using CNC_CAD.SVGTools;

namespace CNC_CAD.GCode
{
    public class GCodePathBuilder : GCodeBuilder2D
    {
        private string _data;
        private Vector _startPoint;
        private Vector _currentPoint;

        public GCodePathBuilder(CncConfig config, string pathData) : base(config)
        {
            _data = pathData;
        }

        protected override List<string> GenerateCommands()
        {
            var commands = new List<string>();
            var separators = @"(?=[MZLHVCSQTAmzlhvcsqta])";
            var tokens = Regex.Split(_data, separators).Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = GetCommandArguments(tokens[0]);
            _startPoint = new Vector(args[0], args[1]);
            _currentPoint = _startPoint;
            commands.AddRange(WithAbsoluteMove(Config, _startPoint)
                .SetHeadDownAtStart(false)
                .SetHeadDownAtEnd(true)
                .Build());
            for (var i = 1; i < tokens.Length; i++)
            {
                commands.AddRange(GetGCodeForCommand(tokens[i]));
            }

            return commands;
        }

        private List<string> GetGCodeForCommand(string command)
        {
            switch (command[0])
            {
                case 'M':
                    return MoveToAbsolute(command);
                case 'm':
                    return MoveToRelative(command);
                case 'L':
                    return LineAbsolute(command);
                case 'l':
                    return LineRelative(command);
                case 'A':
                    return ArcAbsolute(command);
                case 'a':
                    return ArcRelative(command);
                case 'C':
                    return CubicBezierAbsolute(command);
                default:
                    return new List<string>();
            }
        }

        private List<string> MoveToAbsolute(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X = args[0];
            _currentPoint.Y = args[1];
            return WithAbsoluteMove(Config, _currentPoint)
                .SetHeadDownAtStart(false)
                .SetHeadDownAtEnd(true)
                .Build();
        }

        private List<string> MoveToRelative(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X += args[0];
            _currentPoint.Y += args[1];
            return WithAbsoluteMove(Config, _currentPoint)
                .SetHeadDownAtStart(false)
                .SetHeadDownAtEnd(true)
                .Build();
        }

        private List<string> LineAbsolute(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X = args[0];
            _currentPoint.Y = args[1];
            return WithAbsoluteMove(Config, _currentPoint)
                .Build();
        }

        private List<string> LineRelative(string command)
        {
            var args = GetCommandArguments(command);
            _currentPoint.X += args[0];
            _currentPoint.Y += args[1];
            return WithAbsoluteMove(Config, _currentPoint)
                .Build();
        }

        private List<string> ArcAbsolute(string command)
        {
            List<string> commands = new List<string>();
            var args = GetCommandArguments(command);
            var arc = new SvgArc(args, _currentPoint);
            for (double i = 0; Math.Abs(i) <= Math.Abs(arc.Dtetha); i += 10*Math.Sign(arc.Dtetha) * Math.PI / 180d)
            {
                commands.AddRange(WithAbsoluteMove(Config, arc.GetPointOnArcAngle(i)).Build());
            }
            _currentPoint = arc.GetEndpoint();
            commands.AddRange(WithAbsoluteMove(Config, _currentPoint).Build());
            return commands;
        }
        
        private List<string> ArcRelative(string command)
        {
            List<string> commands = new List<string>();
            var args = GetCommandArguments(command);
            args[^1] += _currentPoint.Y;
            args[^2] += _currentPoint.X;
            var arc = new SvgArc(args, _currentPoint);
            for (double i = 0; Math.Abs(i) <= Math.Abs(arc.Dtetha); i += 10*Math.Sign(arc.Dtetha) * Math.PI / 180d)
            {
                commands.AddRange(WithAbsoluteMove(Config, arc.GetPointOnArcAngle(i)).Build());
            }
            _currentPoint = arc.GetEndpoint();
            commands.AddRange(WithAbsoluteMove(Config, _currentPoint).Build());
            return commands;
        }

        private List<string> CubicBezierAbsolute(string command)
        {
            List<string> commands = new List<string>();
            var args = GetCommandArguments(command);
            var cubic = new SvgCubicBezier(args, _currentPoint);
            for (double i = 0; i<=1d; i+=0.01d)
            {
                commands.AddRange(WithAbsoluteMove(Config, cubic.GetPointAt(i)).Build());
            }
            _currentPoint = new Vector(args[^2], args[^1]);
            return commands;
        }

        public static double[] GetCommandArguments(string command)
        {
            var separators = "[\\s,]+";
            var tokens = Regex.Split(command.Substring(1, command.Length - 1), separators)
                .Where(t => !string.IsNullOrEmpty(t)).ToArray();
            var args = new double[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
                args[i] = double.Parse(tokens[i]);
            return args;
        }
    }
}