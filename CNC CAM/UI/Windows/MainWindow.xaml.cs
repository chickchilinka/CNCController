using System.Windows;
using System.Windows.Input;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Configuration.View;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Operations;
using CNC_CAM.Tools;
using CNC_CAM.UI.CustomWPFElements;
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
        public WorkspaceFacade WorkspaceFacade { get; }
        public OperationsController OperationsController { get; }
        private readonly Logger _logger;
        private CurrentConfiguration _currentConfiguration;
        private SignalBus _signalBus;
        private IContainer _container;

        public MainWindow(IContainer container)
        {
            _currentConfiguration = container.Resolve<CurrentConfiguration>();
            InitializeComponent();
            _container = container;
            _logger = Logger.CreateFor(this);
            _logger.Log("Created MainWindow");
            
            WorkspaceFacade = container.Resolve<WorkspaceFacade>();
            OperationsController = container.Resolve<OperationsController>();
            HierarchyStackPanel.Children.Add(WorkspaceFacade.HierarchyView);
            WorkspaceScrollView.Content = WorkspaceFacade.WorkspaceView;
            
            _signalBus = container.Resolve<SignalBus>();
            _signalBus.Subscribe<WpfSignals.MouseMoved>(moved =>
            {
                var mmPosition = _currentConfiguration.ConvertVectorToPhysical(moved.Position);
                MousePositionText.Content = string.Format(Const.Formatters.MousePositionFormatMM, (int)mmPosition.X,
                    (int)mmPosition.Y);
            });
        }

        private void ImportSvg_OnClick(object sender, RoutedEventArgs e)
        {
            OperationsController.LaunchOperation(new LoadSvgOperation(WorkspaceFacade));
        }

        private void StartDummyDraw_Click(object sender, RoutedEventArgs e)
        {
            OperationsController.LaunchOperation(new DrawOperation(_container));
        }
        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            OperationsController.Undo();
        }

        private void GridSize_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var window = _container.Resolve<ManageConfigurationWindow>();
            window.Initialize(typeof(WorksheetConfig));
            window.ShowDialog();
        }

        public string GetWorksheetInfo()
        {
            var config = _currentConfiguration.Get<WorksheetConfig>();
            return $"Размер сетки X:{config.GridSizeX} Y:{config.GridSizeY}, рабочая область ({config.MinX} {config.MinY} {config.MaxX} {config.MaxY})";
        }

        private void Redo_OnClick(object sender, RoutedEventArgs e)
        {
            OperationsController.Redo();
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            var window = _container.Resolve<ManageConfigurationWindow>();
            window.Initialize();
            window.ShowDialog();
        }
    }
}