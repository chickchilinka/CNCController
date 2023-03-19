using System.Collections.Generic;
using CNC_CAM.Shapes;

namespace CNC_CAM.Machine;

public class MachineSignals
{
    public class ExportShapes
    {
        public List<Shape> Shapes { get; }
        public bool TestMode { get; }

        public ExportShapes(List<Shape> shapes, bool testMode)
        {
            TestMode = testMode;
            Shapes = shapes;
        }
    }
}