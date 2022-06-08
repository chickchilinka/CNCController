using System.Collections.Generic;
using CNC_CAD.Configs;
using CNC_CAD.GCode;
using WpfShape = System.Windows.Shapes.Shape;
namespace CNC_CAD.Shapes
{
    public class SvgGroupElement:SvgElement, IGroup<SvgElement>
    {
        public List<SvgElement> Children { get; }= new();
        public override List<WpfShape> GetControlShapes()
        {
            var shapesList = new List<WpfShape>();
            foreach (var shape in Children)
            {
                shapesList.AddRange(shape.GetControlShapes());
            }
            return shapesList;
        }

        public override List<GCodeCommand> GenerateGCodeCommands(CncConfig config)
        {
            var commands = new List<GCodeCommand>();
            foreach (var shape in Children)
            {
                commands.AddRange(shape.GenerateGCodeCommands(config));
            }
            return commands;
        }
    }
}