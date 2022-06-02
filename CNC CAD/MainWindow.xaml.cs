using System.Collections.Generic;
using System.Windows;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Curves;
using CNC_CAD.DrawShapeWindows;
using CNC_CAD.Operations;
using CNC_CAD.Shapes;
using CNC_CAD.Tools;
using CNC_CAD.Workspaces;

namespace CNC_CAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Logger _logger;
        private readonly Workspace _workspace;
        private readonly OperationsHistory _operationsHistory;
        public MainWindow()
        {
            InitializeComponent();
            _logger = Logger.CreateFor(this);
            _logger.Log("Created MainWindow");
            _workspace = new Workspace();
            _operationsHistory = new OperationsHistory();
            WorkspaceScrollView.Content = _workspace.Workspace2D;
        }

        private void ButtonCreateArc3Points_OnClick(object sender, RoutedEventArgs e)
        {
            Window window = MakeShapeWindowBuilder.Create((vectors, fields) =>
                {

                })
                .AddVector2Field("1st point")
                .AddVector2Field("2nd point")
                .AddVector2Field("3rd point")
                .Build();
            window.Show();
        }

        private void ImportSvg_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new LoadSvgOperation(_workspace));
        }

        private void StartDummyDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendsCommandToMachineOperation(new DummyCncController2D(), _workspace, App.currentCNCConfig));
        }

        private void StartCncDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendsCommandToMachineOperation(new SimpleCncSerialController2D(App.currentCNCConfig), _workspace, App.currentCNCConfig));
        }
    }
}