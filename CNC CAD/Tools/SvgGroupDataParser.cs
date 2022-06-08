using System.Collections.Generic;
using System.Xml;
using CNC_CAD.Shapes;

namespace CNC_CAD.Tools
{
    public class SvgGroupDataParser<T>:SvgElementParser<T> where T:SvgGroupElement, new() 
    {
        public override T Create(XmlElement element)
        {
            var group = base.Create(element);
            group.Children.AddRange(SvgParser.GetRootGroups(element, group));
            group.Children.AddRange(SvgParser.GetRootPaths(element, group));
            return group;
        }
        public static List<PathShape> GetRootPaths(XmlElement element, SvgElement parent=null)
        {
            XmlNodeList pathList = element.GetElementsByTagName("path");
            var dataParser = new SvgPathDataParser();
            List<PathShape> pathShapes = new();
            foreach (XmlElement path in pathList)
            {
                if (path.ParentNode != null && path.ParentNode.Equals(element))
                {
                    pathShapes.Add(dataParser.Create(path));
                    pathShapes[^1].Parent = parent;
                }
            }
            return pathShapes;
        }

        public static List<SvgGroupElement> GetRootGroups(XmlElement element, SvgElement parent=null)
        {
            List<SvgGroupElement> groups = new();
            XmlNodeList groupList = element.GetElementsByTagName("g");
            var dataParser = new SvgGroupDataParser<SvgGroupElement>();
            foreach (XmlElement group in groupList)
            {
                if (@group.ParentNode != null && @group.ParentNode.Equals(element))
                {
                    groups.Add(dataParser.Create(group));
                    groups[^1].Parent = parent;
                }
            }
            return groups;
        }
    }
}