using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.Workspaces.Hierarchy;
using DryIoc;

namespace CNC_CAM.Workspaces.View
{
    public partial class WorkspaceView : UserControl
    {
        private Dictionary<WorkspaceElement, UIElement> _views = new();
        private SignalBus _signalBus;
        private WorkspaceElementViewFactory _workspaceElementViewFactory;
        private MoveAdorner _currentMoveAdorner;
        private ScaleAdorner _currentScaleAdorner;
        private CurrentConfiguration _currentConfiguration;
        
        public WorkspaceView(SignalBus signalBus, WorkspaceElementViewFactory viewFactory,
            CurrentConfiguration currentConfiguration)
        {
            InitializeComponent();
            _currentConfiguration = currentConfiguration;
            _currentConfiguration.OnCurrentConfigChanged += ConfigurationChanged;
            _workspaceElementViewFactory = viewFactory;
            _signalBus = signalBus;
            MouseMove += MouseMoved;
            ConfigurationChanged(typeof(WorksheetConfig));
        }

        private void ConfigurationChanged(Type configType)
        {
            if (configType == typeof(WorksheetConfig))
            {
                var worksheetConfig = _currentConfiguration.Get<WorksheetConfig>();
                SafetyArea.Margin = new Thickness(worksheetConfig.MinX / worksheetConfig.Scale,
                    worksheetConfig.MinY / worksheetConfig.Scale, 0, 0);
                SafetyArea.Height = (worksheetConfig.MaxY - worksheetConfig.MinY) / worksheetConfig.Scale;
                SafetyArea.Width = (worksheetConfig.MaxX - worksheetConfig.MinX) / worksheetConfig.Scale;
                var gridSize = new Vector(worksheetConfig.GridSizeX / worksheetConfig.Scale,
                    worksheetConfig.GridSizeY / worksheetConfig.Scale);
                GridDrawingBrush.Viewport = new Rect(0, 0, gridSize.X, gridSize.Y);
                GridRect.Rect = new Rect(0, 0, gridSize.X, gridSize.Y);
            }
        }

        private void MouseMoved(object obj, MouseEventArgs mouseEventArgs)
        {
            _signalBus.Fire(new WpfSignals.MouseMoved(mouseEventArgs.GetPosition(this).ToVector()));
        }

        public void AddElement<TElement>(TElement element) where TElement : WorkspaceElement
        {
            var view = _workspaceElementViewFactory.Create(element) as UIElement;
            _views.Add(element, view);
            Canvas.Children.Add(view);
        }

        public void RemoveElement<TElement>(TElement element) where TElement : WorkspaceElement
        {
            if (_views.TryGetValue(element, out var view))
            {
                if (_currentMoveAdorner.AdornedElement == view)
                    DeselectCurrent();
                Canvas.Children.Remove(view);
            }

            _views.Remove(element);
        }

        public void AddShape(Shape shape)
        {
            Canvas.Children.Add(shape);
        }

        public void Select<TElement>(TElement element) where TElement : WorkspaceElement
        {
            if (_currentMoveAdorner != null)
            {
                DeselectCurrent();
            }

            SelectElement(element);
        }

        private void SelectElement(WorkspaceElement workspaceElement)
        {
            if (!_views.TryGetValue(workspaceElement, out var view))
                return;
            _currentMoveAdorner = new MoveAdorner(view);
            _currentScaleAdorner = new ScaleAdorner(view);
            Canvas.Children.Add(_currentMoveAdorner);
            Canvas.Children.Add(_currentScaleAdorner);
        }

        private void DeselectCurrent()
        {
            _currentMoveAdorner.Dispose();
            _currentScaleAdorner.Dispose();
            Canvas.Children.Remove(_currentMoveAdorner);
            Canvas.Children.Remove(_currentScaleAdorner);
        }

        public void RemoveShape(Shape shape)
        {
            Canvas.Children.Remove(shape);
        }
    }
}