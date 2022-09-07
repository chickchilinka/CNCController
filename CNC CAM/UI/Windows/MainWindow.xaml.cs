using System.Windows;
using System.Windows.Input;
using CNC_CAM.Machine.CNC.Controllers;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Observers;
using CNC_CAM.Operations;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;
using CNC_CAM.UI.DrawShapeWindows;
using CNC_CAM.Workspaces;

namespace CNC_CAM.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private readonly Logger _logger;
        private readonly Workspace _workspace;
        private readonly OperationsHistory _operationsHistory;
        private MouseObserver _mouseObserver;

        public MainWindow(CncConfig config)
        {
            InitializeComponent();
            _logger = Logger.CreateFor(this);
            _logger.Log("Created MainWindow");
            _workspace = new Workspace();
            _operationsHistory = new OperationsHistory();
            WorkspaceScrollView.Content = _workspace.Workspace2D;
            GridSize.Text = $"Grid size:{_workspace.Workspace2D.GridSize}mm";
            SignalBus.Subscribe<WpfSignals.SetGridSize>((size => GridSize.Text = $"Grid size:{size.GridSize}mm"));
            SignalBus.Subscribe<WpfSignals.MouseMoved>(moved =>
            {
                var mmPosition = config.ConvertVectorToPhysical(moved.Position);
                MousePositionText.Text = string.Format(Const.Formatters.MousePositionFormatMM, (int)mmPosition.X,
                    (int)mmPosition.Y);
            });
        }

        private void ButtonCreateArc3Points_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ImportSvg_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new LoadSvgOperation(_workspace));
        }

        private void StartDummyDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(new DummyCncController2D(), _workspace,
                App.CurrentCNCConfig));
        }

        private void StartCncDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(
                new SimpleCncSerialController2D(App.CurrentCNCConfig), _workspace, App.CurrentCNCConfig));
        }

        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.Undo();
        }

        private void Configure_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow window = new ConfigurationWindow(App.CurrentCNCConfig);
            window.Show();
        }

        private void GridSize_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var window = MakeShapeWindowBuilder
                .Create(
                    (vectors, decimals) =>
                    {
                        SignalBus.Fire(new WpfSignals.SetGridSize((int)decimals["Grid size"]));
                    },
                    () => { }).AddSimpleFloatField("Grid size", defaultValue: _workspace.Workspace2D.GridSize).Build();
            window.Show();
        }
    }
}