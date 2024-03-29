using System.Collections.Generic;
using System.Windows;
using CNC_CAM.Shapes;
using CNC_CAM.SVG.Elements;
using CNC_CAM.Tools;

namespace CNC_CAM.Tests
{
    public class PathConcatTest:ITest
    {
        Logger _logger;
        public PathConcatTest()
        {
            _logger = Logger.CreateFor(this);
        }

        public void Test()
        {
            //Test shapes
            var s1 = new SvgPath("", "s1", new List<ICurve>())
            {
                StartPoint = new Vector(0, 0),
                EndPoint = new Vector(1, 1)                    
            };
            var s2 = new SvgPath("", "s2", new List<ICurve>())
            {
                StartPoint = new Vector(1, 1),
                EndPoint = new Vector(2, 2)                    
            };
            var s3 = new SvgPath("", "s3", new List<ICurve>())
            {
                StartPoint = new Vector(3, 3),
                EndPoint = new Vector(1, 1)                    
            };
            var s4 = new SvgPath("", "s1", new List<ICurve>())
            {
                StartPoint = new Vector(1, 1),
                EndPoint = new Vector(3, 3)                    
            };
            var list = new List<SvgPath>
            {
                s1,
                s2,
                s3,
                s4
            };
            SvgRoot.ConcatShapes(list);
            _logger.Log($"length:{list.Count}");
            foreach (var shape in list)
            {
                _logger.Log(shape);
            }
        }
    }
}