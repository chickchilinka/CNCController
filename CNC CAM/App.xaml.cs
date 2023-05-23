using System.Windows;
using CNC_CAM.Base;
using CNC_CAM.Configuration;
using CNC_CAM.Tools;
using CNC_CAM.UI.Windows;
using DryIoc;

namespace CNC_CAM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool ShowConsole = false;
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
            Exit += OnExit;
            _mainWindow = new MainWindow(_container);
            _mainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _signalBus.Fire(new ConfigurationSignals.SaveConfigs());
            _signalBus.Fire(new AppSignals.Exit());
        }
    }
}