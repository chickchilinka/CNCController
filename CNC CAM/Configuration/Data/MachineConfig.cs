using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Data.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn), Name("Станок")]
public class MachineConfig : BaseConfig
{
    [ConfigProperty("Название оси X")]
    public string AxisX
    {
        get => _axisX;
        set
        {
            _axisX = value;
            HandleChange();
        }
    }

    [ConfigProperty("Название оси Y")]
    public string AxisY
    {
        get => _axisY;
        set
        {
            _axisY = value;
            HandleChange();
        }
    }

    [ConfigProperty("Название оси Z")]
    public string AxisZ
    {
        get => _axisZ;
        set
        {
            _axisZ = value;
            HandleChange();
        }
    }
    
    [ConfigProperty("Скорость инструмента", "Линейная скорость инструмента")]
    public double BaseFeedRate
    {
        get => _baseFeedRate;
        set
        {
            _baseFeedRate = value;
            HandleChange();
        }
    }
    
    [ConfigProperty("Порт")]
    public string ComPort
    {
        get => _comPort;
        set
        {
            _comPort = value;
            HandleChange();
        }
    }
    [ConfigProperty("Baud Rate")]
    public int BaudRate
    {
        get => _baudRate;
        set
        {
            _baudRate = value;
            HandleChange();
        }
    }

    
    [JsonProperty, DBField] private string _comPort = "COM5";
    [JsonProperty, DBField] private int _baudRate = 115200;
    [JsonProperty, DBField] private double _baseFeedRate = 3000;
    [JsonProperty, DBField] private string _axisX = "X";
    [JsonProperty, DBField] private string _axisY = "Y";
    [JsonProperty, DBField] private string _axisZ = "Z";
    [JsonProperty, DBField] private int _stepsPerMmX = 500;
    [JsonProperty, DBField] private int _stepsPerMmY = 500;
    [JsonProperty, DBField] private int _stepsPerMmZ = 500;
}