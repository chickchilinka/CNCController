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
    
    public class SetSafetyAreaSize
    {
        public float Width { get; }
        public float Height { get; }

        public SetSafetyAreaSize(float width, float height)
        {
            Width = width;
            Height = height;
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