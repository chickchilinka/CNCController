using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Observers;
using CNC_CAM.Operations;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.UI.DrawShapeWindows;
using CNC_CAM.Workspaces;
using CNC_CAM.Workspaces.Hierarchy.View;
using DryIoc;

namespace CNC_CAM.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private readonly Logger _logger;
        private readonly WorkspaceFacade _workspaceFacade;
        private readonly OperationsHistory _operationsHistory;
        private MouseObserver _mouseObserver;
        private CurrentConfiguration _currentConfiguration;
        private SignalBus _signalBus;
        private IContainer _container;
        public string SomeText => "SomeText";

        public MainWindow(IContainer container)
        {
            InitializeComponent();
            _container = container;
            HierarchyStackPanel.Children.Add(_container.Resolve<HierarchyView>());
            _currentConfiguration = container.Resolve<CurrentConfiguration>();
            _logger = Logger.CreateFor(this);
            _logger.Log("Created MainWindow");
            _workspaceFacade = container.Resolve<WorkspaceFacade>();
            _operationsHistory = container.Resolve<OperationsHistory>();
            WorkspaceScrollView.Content = _workspaceFacade.Workspace2D;
            SafeAreaWidth.Value = _workspaceFacade.Workspace2D.SafetyArea.Width.ToString(CultureInfo.InvariantCulture);
            SafeAreaHeight.Value = _workspaceFacade.Workspace2D.SafetyArea.Height.ToString(CultureInfo.InvariantCulture);
            SafeAreaWidth.OnChangedNumeric +=
                (_) => _workspaceFacade.Workspace2D.SafetyRect = new Rect(0, 0, SafeAreaWidth.NumericValue, SafeAreaHeight.NumericValue);
            SafeAreaHeight.OnChangedNumeric +=
                (_) => _workspaceFacade.Workspace2D.SafetyRect = new Rect(0, 0, SafeAreaWidth.NumericValue, SafeAreaHeight.NumericValue);
            GridSize.Text = $"Grid size:{_workspaceFacade.Workspace2D.GridSize}mm";
            _signalBus = container.Resolve<SignalBus>();
            _signalBus.Subscribe<WpfSignals.SetGridSize>((size => GridSize.Text = $"Grid size:{size.GridSize}mm"));
            _signalBus.Subscribe<WpfSignals.MouseMoved>(moved =>
            {
                var mmPosition = _currentConfiguration.ConvertVectorToPhysical(moved.Position);
                MousePositionText.Text = string.Format(Const.Formatters.MousePositionFormatMM, (int)mmPosition.X,
                    (int)mmPosition.Y);
            });
        }

        private void ButtonCreateArc3Points_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ImportSvg_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new LoadSvgOperation(_workspaceFacade));
        }

        private void StartDummyDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(_container, new DummyCncController2D(), _workspaceFacade,
                _currentConfiguration));
        }

        private void StartCncDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(_container, 
                new SimpleCncSerialController2D(_currentConfiguration), _workspaceFacade, _currentConfiguration));
        }

        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.Undo();
        }

        private void Configure_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow window = new ConfigurationWindow(_currentConfiguration);
            window.Show();
        }

        private void GridSize_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var window = GenericWindowWithFieldsBuilder
                .Create(
                    (values) =>
                    {
                        _currentConfiguration.GetCurrentConfig<WorksheetConfig>().GridSizeX = (float)values["Grid size"];
                        _currentConfiguration.GetCurrentConfig<WorksheetConfig>().GridSizeY = (float)values["Grid size"];
                        _signalBus.Fire(new WpfSignals.SetGridSize((int)(float)values["Grid size"]));
                    },
                    () => { }).AddSimpleFloatField("Grid size", defaultValue: _workspaceFacade.Workspace2D.GridSize)
                .WithTitle("Grid size").Build();
            window.Show();
        }

        private void SafetyArea_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var window = GenericWindowWithFieldsBuilder
                .Create(
                    (values) =>
                    {
                        _signalBus.Fire(new WpfSignals.SetSafetyAreaSize((float)values["Width"], (float)values["Height"]));
                    },
                    () => { })
                .WithTitle("Safety area")
                .AddWidthHeightField("Safety area size",
                    new Vector2((float)_workspaceFacade.Workspace2D.SafetyArea.Width,
                        (float)_workspaceFacade.Workspace2D.SafetyArea.Height)).Build();
            window.Show();
        }
    }
}