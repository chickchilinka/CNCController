using System.Windows;

namespace CNC_CAM.UI.CustomWPFElements;

public class WpfSignals
{
    public class SetGridSize
    {
        public int GridSize { get; }

        public SetGridSize(int gridSize)
        {
            GridSize = gridSize;
        }
    }

    public class MouseMoved
    {
        public Vector Position { get; }

        public MouseMoved(Vector position)
        {
            Position = position;
        }
    }
}