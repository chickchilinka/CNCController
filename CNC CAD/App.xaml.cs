using System.Windows;
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

        public App()
        {
            if (ShowConsole)
            {
                ConsoleManager.Show();
            }
        }
    }
}