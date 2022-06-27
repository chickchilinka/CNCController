using System;
using System.Collections.Generic;
using System.Management;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;
using CNC_CAD.GCode;
using CNC_CAD.Tools;

namespace CNC_CAD.Windows;

public partial class ConfigurationWindow : Window
{
    private CncConfig _config;
    private Logger _logger;
    private ConfigurationWindow()
    {
        InitializeComponent();
    }

    public ConfigurationWindow(CncConfig configToEdit):this()
    {
        _config = configToEdit;
        ZDown.Value = configToEdit.HeadDown.ToString();
        ZUp.Value = configToEdit.HeadUp.ToString();
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
        _config.HeadDown = ZDown.NumericValue;
        _config.HeadUp = ZUp.NumericValue;
        _config.COMPort = PortsList.SelectionBoxItem.ToString();
        _config.BaudRate = int.Parse(BaudRate.Value);
    }

    private void PortsList_OnSelected(object sender, EventArgs eventArgs)
    {
        ShowDeviceInfo(PortsList.SelectionBoxItem.ToString());
    }
    private void ButtonTestZDown_OnClick(object sender, RoutedEventArgs e)
    {
        ApplyConfig();
        SendTestZCommand(_config.HeadDown);
    }
    private void ButtonTestZUp_OnClick(object sender, RoutedEventArgs e)
    {
        ApplyConfig();
        SendTestZCommand(_config.HeadUp);
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