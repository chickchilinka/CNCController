using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;
using Transform = CNC_CAD.Curves.Transform;

namespace CNC_CAD.Shapes
{
    public abstract class Shape:Transform
    {
        public double StrokeWidth { get; set; }
        protected List<WPFShape> WpfShapes = new List<WPFShape>();
        public abstract List<GCodeCommand> GenerateGCodeCommands(CncConfig config);

        public virtual List<WPFShape> GetControlShapes()
        {
            return WpfShapes;
        }
    }
}