using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Shapes;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.Workspaces.View;
using DryIoc;
using Shape = CNC_CAM.Shapes.Shape;

namespace CNC_CAM.UI.Windows
{

    public partial class ExportWindow : Window
    {
        private SignalBus _signalBus;
        private Workspace2D _workspace2D;
        private CurrentConfiguration _currentConfiguration;
        private List<ICurve> _curvesList;
        private List<Line> _lines = new();
        private List<Shape> _shapes = new();

        public ExportWindow(Workspace2D workspace2D, CurrentConfiguration currentConfiguration, SignalBus signalBus)
        {
            InitializeComponent();
            _signalBus = signalBus;
            _workspace2D = workspace2D;
            _currentConfiguration = currentConfiguration;
            _currentConfiguration.OnCurrentConfigChanged += OnCurrentConfigChanged;
            WorkspaceScrollView.Content = _workspace2D;
        }

        private void OnCurrentConfigChanged(Type obj)
        {
            Draw();
        }

        public void Initialize(List<Shape> shapes, List<ICurve> curvesList)
        {
            _shapes = shapes;
            _curvesList = curvesList;
            Draw();
        }

        private void Draw()
        {
            var accuracy = _currentConfiguration.GetCurrentConfig<AccuracySettings>();
            foreach (var line in _lines)
            {
                _workspace2D.RemoveShape(line);
            }
            _lines.Clear();
            foreach (var curve in _curvesList)
            {
                Vector curPoint = curve.ToGlobalPoint(curve.StartPoint);
                foreach (var lineEnd in curve.Linearize(accuracy))
                {
                    var line = new Line()
                    {
                        X1 = curPoint.X,
                        X2 = lineEnd.X,
                        Y1 = curPoint.Y,
                        Y2 = lineEnd.Y,
                        StrokeThickness = 1,
                        Stroke = Brushes.Black
                    };
                    _lines.Add(line);
                    _workspace2D.AddShape(line);
                    curPoint = lineEnd;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _currentConfiguration.OnCurrentConfigChanged -= OnCurrentConfigChanged;
        }

        private void TestExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _signalBus.Fire(new MachineSignals.ExportShapes(_shapes, true));
        }

        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _signalBus.Fire(new MachineSignals.ExportShapes(_shapes, false));
        }
    }
}