using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.Workspaces.Hierarchy;
using DryIoc;

namespace CNC_CAM.Workspaces.View
{
    public partial class Workspace2D : UserControl
    {
        private Dictionary<WorkspaceElement, UIElement> _views = new();
        private int _gridSize = 50;
        private Rect _safetyRect;
        private SignalBus _signalBus;
        private WorkspaceElementViewFactory _workspaceElementViewFactory;
        private SimpleAdorner _currentAdorner;

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

        public Workspace2D(SignalBus signalBus, WorkspaceElementViewFactory viewFactory)
        {
            InitializeComponent();
            _workspaceElementViewFactory = viewFactory;
            _signalBus = signalBus;
            _signalBus.Subscribe<WpfSignals.SetGridSize>((signal) => GridSize = signal.GridSize);
            _signalBus.Subscribe<WpfSignals.SetSafetyAreaSize>((signal) => SafetyRect = new Rect(0,0, signal.Width, signal.Height));
            MouseMove += MouseMoved;
        }

        private void MouseMoved(object obj, MouseEventArgs mouseEventArgs)
        {
            _signalBus.Fire(new WpfSignals.MouseMoved(mouseEventArgs.GetPosition(this).ToVector()));
        }
        
        public void AddElement<TElement>(TElement element) where TElement:WorkspaceElement
        {
            var view = _workspaceElementViewFactory.Create(element) as UIElement;
            _views.Add(element, view);
            Canvas.Children.Add(view);
        }

        public void RemoveElement<TElement>(TElement element) where TElement : WorkspaceElement
        {
            if(_views.TryGetValue(element, out var view))
                Canvas.Children.Remove(view);
            _views.Remove(element);
        }
        public void AddShape(Shape shape)
        {
            Canvas.Children.Add(shape);
        }

        public void Select<TElement>(TElement element) where TElement : WorkspaceElement
        {
            if (_currentAdorner != null)
                Canvas.Children.Remove(_currentAdorner);
            if (_views.TryGetValue(element, out var view))
            {
                _currentAdorner = new SimpleAdorner(view);
                Canvas.Children.Add(_currentAdorner);
            }
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