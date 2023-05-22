using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine;
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
        private WorkspaceView _workspaceView;
        private CurrentConfiguration _currentConfiguration;
        private List<ICurve> _curvesList;
        private List<Line> _lines = new();
        private List<Shape> _shapes = new();

        public ExportWindow(WorkspaceView workspaceView, CurrentConfiguration currentConfiguration, SignalBus signalBus)
        {
            InitializeComponent();
            _signalBus = signalBus;
            _workspaceView = workspaceView;
            _currentConfiguration = currentConfiguration;
            _currentConfiguration.OnCurrentConfigChanged += OnCurrentConfigChanged;
            WorkspaceScrollView.Content = _workspaceView;
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
            var settings = _currentConfiguration.Get<UserSettings>();
            foreach (var line in _lines)
            {
                _workspaceView.RemoveShape(line);
            }
            _lines.Clear();
            for (int i = 0; i < _curvesList.Count; i++)
            {
                var curve = _curvesList[i];
                Vector curPoint = curve.ToGlobalPoint(curve.StartPoint);
                if (i > 0)
                {
                    var moveLine = new Line()
                    {
                        X1 = _lines[^1].X2,
                        Y1 = _lines[^1].Y2,
                        X2 = curPoint.X,
                        Y2 = curPoint.Y,
                        StrokeThickness = 1,
                        Stroke = Brushes.Red
                    };
                    _lines.Add(moveLine);
                    _workspaceView.AddShape(moveLine);
                }
                else
                {
                    var startPoint = _currentConfiguration.ConvertVectorToPhysical(new Vector(0, 0))/_currentConfiguration.Get<WorksheetConfig>().Scale;
                    var moveLine = new Line()
                    {
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = curPoint.X,
                        Y2 = curPoint.Y,
                        StrokeThickness = 1,
                        Stroke = Brushes.Red
                    };
                    _lines.Add(moveLine);
                    _workspaceView.AddShape(moveLine);
                }
                foreach (var lineEnd in curve.Linearize(settings.Accuracy))
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
                    _workspaceView.AddShape(line);
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