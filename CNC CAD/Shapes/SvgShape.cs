using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using CNC_CAD.Configs;
using WPFShape = System.Windows.Shapes.Shape;
using CNC_CAD.GCode;
using CNC_CAD.Tools;

namespace CNC_CAD.Shapes
{
    public class SvgShape:Shape
    {
        public List<Shape> _shapes { get; }= new();
        private SvgPathDataParser _dataParser;

        public SvgShape(string xmlData) : 
            this(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xmlData ?? "")))){}
        
        public SvgShape(XmlReader xmlReader)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlReader);
            XmlNodeList pathList = document.GetElementsByTagName("path");
            _dataParser = new SvgPathDataParser();
            List<PathShape> pathShapes = new();
            foreach (XmlElement element in pathList)
            {
                pathShapes.Add(_dataParser.CreatePath(element));
            }
            //ConcatShapes(pathShapes);
            _shapes.AddRange(pathShapes);
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

        public static void ConcatShapes(List<PathShape> shapes)
        {
            bool found = false;
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i] != null)
                {
                    for (int j = 0; j < shapes.Count; j++)
                    {
                        if (i != j && shapes[j] != null && shapes[j].StartPoint == shapes[i].EndPoint)
                        {
                            found = true;
                            shapes[i].Concat(shapes[j]);
                            shapes.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
            if (found)
            {
                ConcatShapes(shapes);
            }
        }
    }
}