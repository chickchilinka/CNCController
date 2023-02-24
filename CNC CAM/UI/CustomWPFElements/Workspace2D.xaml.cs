using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using CNC_CAM.Tools;

namespace CNC_CAM.UI.CustomWPFElements
{
    public partial class Workspace2D : UserControl
    {
        private int _gridSize = 50;
        private Rect _safetyRect;

        public int GridSize
        {
            get => _gridSize;
            set
            {
                _gridSize = value;
                GridDrawingBrush.Viewport = new Rect(0, 0, _gridSize, _gridSize);
                GridRect.Rect = new Rect(0, 0, _gridSize, _gridSize);
            }
        }
        
        public Rect SafetyRect
        {
            get => _safetyRect;
            set
            {
                _safetyRect = value;
                SafetyArea.Height = _safetyRect.Height;
                SafetyArea.Width = _safetyRect.Width;
            }
        }

        public Workspace2D()
        {
            InitializeComponent();
            SignalBus.Subscribe<WpfSignals.SetGridSize>((signal) => GridSize = signal.GridSize);
            SignalBus.Subscribe<WpfSignals.SetSafetyAreaSize>((signal) => SafetyRect = new Rect(0,0, signal.Width, signal.Height));
            MouseMove += MouseMoved;
        }

        public void MouseMoved(object obj, MouseEventArgs mouseEventArgs)
        {
            SignalBus.Fire(new WpfSignals.MouseMoved(mouseEventArgs.GetPosition(this).ToVector()));
        }

        public void AddShape(Shape shape)
        {
            Canvas.Children.Add(shape);
        }

        public void AddShapes(params Shape[] shapes)
        {
            foreach (var shape in shapes)
            {
                Canvas.Children.Add(shape);
            }
        }

        public void RemoveShape(Shape shape)
        {
            Canvas.Children.Remove(shape);
        }

        public void ClearShapes()
        {
            Canvas.Children.Clear();
        }
    }
}