using System.IO.Ports;
using System.Windows;

namespace CNC_CAM.Configuration.View;

public partial class ConfigurationMasterWindow : Window
{
    #region Connection

    private string[] _portValues;

    public string[] Ports => SerialPort.GetPortNames();

    #endregion

    public ConfigurationMasterWindow()
    {
        InitializeComponent();
    }
}