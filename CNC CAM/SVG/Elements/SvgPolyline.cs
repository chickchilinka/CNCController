using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Shapes;
using Shape = System.Windows.Shapes.Shape;

namespace CNC_CAM.SVG.Elements;

public class SvgPolyline : SvgElement, ICurve
{
    public readonly List<Vector> _points = new();
    private Polyline _wpfPolyline;
    public SvgPolyline()
    {
        _wpfPolyline = new Polyline()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };
        WpfShapes.Add(_wpfPolyline);
    }

    public override List<Shape> GetControlShapes()
    {
        _wpfPolyline.Points = new PointCollection(_points.Select(vector => new Point(vector.X, vector.Y)));
        return WpfShapes;
    }

    public override List<GCodeCommand> GenerateGCodeCommands(CurrentConfiguration config)
    {
        List<string> commands = new List<string>();
        List<Vector> points = Linearize(config.GetCurrentConfig<AccuracySettings>());
        if (points.Count == 0) return new List<GCodeCommand>();
        commands.AddRange(GCodeBuilder2D.WithAbsoluteMove(config, points[0]).SetHeadDownAtStart(false)
            .SetHeadDownAtEnd(true).Build());
        for (int i = 1; i < points.Count; i++)
        {
            commands.AddRange(GCodeBuilder2D.WithAbsoluteMove(config, points[i]).SetFastTravel(false).Build());
        }

        return new List<GCodeCommand> { new(commands) };
    }

    public virtual List<Vector> Linearize(AccuracySettings accuracy)
    {
        return _points.Select(ToGlobalPoint).ToList();
    }

    public Vector StartPoint => _points.Count > 0 ? _points[0] : default;

    public Vector EndPoint => _points.Count > 0 ? _points[0] : default;
    public double Length
    {
        get
        {
            if (_points.Count == 0)
                return 0;
            double sum = 0;
            Vector lastPoint = _points[0];
            for (int i = 1; i < _points.Count; i++)
            {
                sum += (lastPoint - _points[i]).Length;
                lastPoint = _points[i];
            }

            return sum;
        }
    }
}