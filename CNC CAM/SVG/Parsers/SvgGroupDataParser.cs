using System.Collections.Generic;
using System.Xml;
using CNC_CAM.SVG.Elements;

namespace CNC_CAM.SVG.Parsers
{
    public class SvgGroupDataParser<T>:SvgElementParser<T> where T:SvgGroupElement, new() 
    {
        public override T Create(XmlElement element)
        {
            var group = base.Create(element);
            group.Children.AddRange(GetChildren(element));
            return group;
        }

        private List<SvgElement> GetChildren(XmlElement svgElement, SvgElement parent=null)
        {
            var list = new List<SvgElement>();
            foreach (XmlElement childNode in svgElement)
            {
                var element = SvgParsersFactory.Parse(childNode);
                if (element != null)
                {
                    element.Parent = parent;
                    list.Add(element);
                }
            }
            return list;
        }
    }
}