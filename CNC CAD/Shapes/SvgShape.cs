using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Shapes;
using System.Xml;
using CNC_CAD.CNC.Controllers;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;

namespace CNC_CAD.Shapes
{
    public class SvgShape:Shape
    {
        private List<Shape> _shapes = new List<Shape>();
        public SvgShape(XmlReader xmlReader)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlReader);
            XmlNodeList pathList = document.GetElementsByTagName("path");
            foreach (XmlElement element in pathList)
            {
                _shapes.Add(new PathShape(element.GetAttribute("d"), element.GetAttribute("id") ?? (App.ShapeID++).ToString()));
            }
            //TODO: Implement more svg shapes
        }
        
        public override List<GCodeCommand> GenerateGCodeCommands(CncConfig config)
        {
            var commands = new List<GCodeCommand>();
            foreach (var shape in _shapes)
            {
                commands.AddRange(shape.GenerateGCodeCommands(config));
            }
            return commands;
        }

        public override List<WPFShape> GetControlShapes()
        {
            var shapesList = new List<WPFShape>();
            foreach (var shape in _shapes)
            {
                shapesList.AddRange(shape.GetControlShapes());
            }
            return shapesList;
        }

        public override void Move(Vector2 delta)
        {
            throw new System.NotImplementedException();
        }

        public override void Scale(Vector2 multiplication, Vector2 pivot)
        {
            throw new System.NotImplementedException();
        }

        public override void Rotate(float angle, Vector2 pivot)
        {
            throw new System.NotImplementedException();
        }
    }
}