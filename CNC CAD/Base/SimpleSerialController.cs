using System;
using System.IO.Ports;
using CNC_CAD.CNC.Controllers;
using CNC_CAD.Configs;

namespace CNC_CAD.Base
{
    public class SimpleSerialController:IDisposable
    {
        private SerialPort _serialPort;

        private SimpleSerialController(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _serialPort.Open();
        }

        public static SimpleSerialController CreateSerialController(CncConfig config)
        {
            return new SimpleSerialController(new SerialPort(config.COMPort, config.BaudRate));
        }

        public void SendString(string message)
        {
            _serialPort.WriteLine(message);
        }

        public void Dispose()
        {
            _serialPort?.Dispose();
        }
    }
}