using System.Windows;
using CNC_CAM.Base;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Configuration.View;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using CNC_CAM.UI.Windows;
using DryIoc;

namespace CNC_CAM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool ShowConsole = true;
        private MainScope _scope;
        private SignalBus _signalBus;
        private Container _container;
        private MainWindow _mainWindow;
        public App()
        {
            if (ShowConsole)
            {
                ConsoleManager.Show();
            }

            _scope = new MainScope();
            _scope.Install();
            _container = _scope.Container;
            _signalBus = _container.Resolve<SignalBus>();
            var window = _container.Resolve<SelectConfigurationWindow>();
            window.Initialize(typeof(CNCControlSettings));
            window.Show();
            Exit += OnExit;
            _mainWindow = new MainWindow(_container);
            _mainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _signalBus.Fire(new AppSignals.Exit());
        }
    }
}