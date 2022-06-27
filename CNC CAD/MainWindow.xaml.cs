using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;
using CNC_CAD.Base;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using CNC_CAD.Curves;
using CNC_CAD.DrawShapeWindows;
using CNC_CAD.Observers;
using CNC_CAD.Operations;
using CNC_CAD.Tools;
using CNC_CAD.Windows;
using CNC_CAD.Workspaces;

namespace CNC_CAD
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
            _mouseObserver = MouseObserver.CreateMouseObserver(config, _workspace.Workspace2D, (text) =>
            {
                MousePositionText.Text = text;
            });
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
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(new DummyCncController2D(), _workspace, App.currentCNCConfig));
        }

        private void StartCncDraw_Click(object sender, RoutedEventArgs e)
        {
            _operationsHistory.LaunchOperation(new SendShapesToMachineOperation(new SimpleCncSerialController2D(App.currentCNCConfig), _workspace, App.currentCNCConfig));
        }

        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            _operationsHistory.Undo();
        }

        private void Configure_OnClick(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow window = new ConfigurationWindow(App.currentCNCConfig);
            window.Show();
        }
    }
}