using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Machine.Configs;
using CNC_CAM.Machine.Controllers;
using CNC_CAM.Machine.GCode;
using CNC_CAM.Tools;

namespace CNC_CAM.UI.Windows;

public partial class ConfigurationWindow : Window
{
    private CurrentConfiguration _config;
    private Logger _logger;
    private ConfigurationWindow()
    {
        InitializeComponent();
    }

    public ConfigurationWindow(CurrentConfiguration configToEdit):this()
    {
        _config = configToEdit;
        ZDown.Value = configToEdit.GetCurrentConfig<CNCHeadSettings>().HeadDown.ToString();
        ZUp.Value = configToEdit.GetCurrentConfig<CNCHeadSettings>().HeadUp.ToString();
        _logger = Logger.CreateFor(this);
        FillComboBox();
    }

    private void FillComboBox()
    {
        foreach (var port in SerialPort.GetPortNames())
        {
            PortsList.Items.Add(new ComboBoxItem()
            {
                Content = port
            });
        }
    }
    
    public void ShowDeviceInfo(string port){
        ManagementObjectCollection ManObjReturn;
        ManagementObjectSearcher ManObjSearch;
        ManObjSearch = new ManagementObjectSearcher("Select * from Win32_SerialPort");
        ManObjReturn = ManObjSearch.Get();

        foreach (ManagementObject ManObj in ManObjReturn)
        {
            if (ManObj["DeviceID"].ToString() == port)
            {
                string data = "Connected device:";
                data += "\n" + ManObj["Name"];
                data += "\n" + ManObj["Description"];
                DeviceName.Text = data;
            }
        }
    }
    private void DoneButton_OnClick(object sender, RoutedEventArgs e)
    {
        ApplyConfig();
        Close();
    }

    private void ApplyConfig()
    {
        _config.GetCurrentConfig<CNCHeadSettings>().HeadDown = ZDown.NumericValue;
        _config.GetCurrentConfig<CNCHeadSettings>().HeadUp = ZUp.NumericValue;
        _config.GetCurrentConfig<CNCConnectionSettings>().ComPort = PortsList.SelectionBoxItem.ToString();
        _config.GetCurrentConfig<CNCConnectionSettings>().BaudRate = int.Parse(BaudRate.Value);
    }

    private void PortsList_OnSelected(object sender, EventArgs eventArgs)
    {
        ShowDeviceInfo(PortsList.SelectionBoxItem.ToString());
    }
    private void ButtonTestZDown_OnClick(object sender, RoutedEventArgs e)
    {
        ApplyConfig();
        SendTestZCommand(_config.GetCurrentConfig<CNCHeadSettings>().HeadDown);
    }
    private void ButtonTestZUp_OnClick(object sender, RoutedEventArgs e)
    {
        ApplyConfig();
        SendTestZCommand(_config.GetCurrentConfig<CNCHeadSettings>().HeadUp);
    }
    private void SendTestZCommand(double zPos)
    {
        var gcodes = new List<GCodeCommand>();
        gcodes.Add(new GCodeCommand(new List<string>{"G90"}));
        gcodes.Add(new GCodeCommand(new List<string>{$"G00 Z{zPos}"}));
        gcodes.Add(new GCodeCommand(new List<string>{"G90"}));
        try
        {
            var controller = new SimpleCncSerialController2D(_config);
            controller.ExecuteGCodeCommands(gcodes);
        }
        catch (Exception ex)
        {
            _logger.Log(ex.StackTrace);
        }
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}