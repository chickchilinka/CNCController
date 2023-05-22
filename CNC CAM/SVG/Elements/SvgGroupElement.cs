using System.Collections.Generic;
using CNC_CAM.Configuration;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Shapes;
using WpfShape = System.Windows.Shapes.Shape;
namespace CNC_CAM.SVG.Elements
{
    public class SvgGroupElement:SvgElement, IGroup<SvgElement>
    {
        public List<SvgElement> Children { get; } = new();
        
        //TODO:Так же отделить сущность от представления
        public override List<WpfShape> GetControlShapes()
        {
            var shapesList = new List<WpfShape>();
            foreach (var shape in Children)
            {
                shapesList.AddRange(shape.GetControlShapes());
            }
            return shapesList;
        }
        //TODO:Так же отделить сущность от представления для станка
        public override List<GCodeCommand> GenerateGCodeCommands(CurrentConfiguration config)
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