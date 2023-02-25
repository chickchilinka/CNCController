using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;
[JsonObject(MemberSerialization.OptIn)]
public class CNCConnectionSettings:BaseConfig
{
    [ConfigProperty("COM Port")]
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

    [JsonProperty]
    private string _comPort = "COM5";
    [JsonProperty]
    private int _baudRate = 115200;
}