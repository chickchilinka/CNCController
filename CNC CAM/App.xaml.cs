using System.Windows;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using CNC_CAM.UI.Windows;

namespace CNC_CAM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool ShowConsole = true;
        public static int ShapeID = 0;
        public static CncConfig CurrentCNCConfig { get; private set; }
        private SimpleSerializer _serializer;
        private MainWindow _mainWindow;
        public App()
        {
            if (ShowConsole)
            {
                ConsoleManager.Show();
            }

            _serializer = new SimpleSerializer();
            LoadData();
            Exit += SaveData;
            _mainWindow = new MainWindow(CurrentCNCConfig);
            _mainWindow.Show();
        }

        private void LoadData()
        {
            CurrentCNCConfig =
                _serializer.Deserialize(Const.Paths.CncConfigFileFullPath, Const.Configs.DefaultCncConfig);
        }

        private void SaveData(object obj, ExitEventArgs eventArgs)
        {
            _serializer.Serialize(Const.Paths.DocumentsPath+Const.Paths.LastSessionFolder,Const.Paths.CncConfigFileName, CurrentCNCConfig);
        }
    }
}