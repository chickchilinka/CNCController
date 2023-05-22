using System;
using System.IO.Ports;
using CNC_CAM.Configuration;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;

namespace CNC_CAM.Base
{
    public class SimpleSerialController:IDisposable
    {
        private SerialPort _serialPort;
        private Logger _logger;

        private SimpleSerialController(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _serialPort.Open();
            _logger = Logger.CreateFor(this);
        }

        public static SimpleSerialController CreateSerialController(CurrentConfiguration config)
        {
            var machineConfig = config.Get<MachineConfig>();
            return new SimpleSerialController(new SerialPort(machineConfig.ComPort, machineConfig.BaudRate));
        }

        public void SendString(string message)
        {
            _serialPort.WriteLine(message);
        }

        public string Read()
        {
            var read = _serialPort.ReadExisting();
            //_logger.Log(read);
            return read;
        }

        public void Dispose()
        {
            _serialPort?.Dispose();
        }
    }
}