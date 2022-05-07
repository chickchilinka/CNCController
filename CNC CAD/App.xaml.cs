using System.Windows;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Tools;

namespace CNC_CAD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool ShowConsole = true;
        public static int ShapeID = 0;
        public static CncConfig currentCNCConfig = new CncConfig();
        public App()
        {
            if (ShowConsole)
            {
                ConsoleManager.Show();
            }
        }
    }
}