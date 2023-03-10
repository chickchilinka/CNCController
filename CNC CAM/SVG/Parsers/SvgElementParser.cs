using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;

namespace CNC_CAM.SVG.Parsers
{
    public abstract class SvgElementParser
    {
        public abstract SvgElement Create(XmlElement element);
    }
    public abstract class SvgElementParser<T>:SvgElementParser where T : SvgElement, new()
    {
        public override T Create(XmlElement element)
        {
            return new T()
            {
                Name = GetId(element),
                TransformationMatrix = element.GetTransformationMatrix()
            };
        }

        

        protected string GetId(XmlElement element)
        {
            return element.GetAttribute("id");
        } 
    }
}