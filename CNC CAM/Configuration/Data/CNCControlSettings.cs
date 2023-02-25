using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn)]
public class CNCControlSettings : BaseConfig
{
    [ConfigProperty("Invert X")]
    public bool InvertX
    {
        get => _invertX;
        set
        {
            _invertX = value;
            HandleChange();
        }
    }
    [ConfigProperty("Invert Y")]
    public bool InvertY
    {
        get => _invertY;
        set
        {
            _invertY = value;
            HandleChange();
        }
    }

    [ConfigProperty("Invert Z")]
    public bool InvertZ
    {
        get => _invertZ;
        set
        {
            _invertZ = value;
            HandleChange();
        }
    }
    [ConfigProperty("X Axis Name")]
    public string AxisX
    {
        get => _axisX;
        set
        {
            _axisX = value;
            HandleChange();
        }
    }

    [ConfigProperty("Y Axis Name")]
    public string AxisY
    {
        get => _axisY;
        set
        {
            _axisY = value;
            HandleChange();
        }
    }

    [ConfigProperty("Z Axis Name")]
    public string AxisZ
    {
        get => _axisZ;
        set
        {
            _axisZ = value;
            HandleChange();
        }
    }
    
    [ConfigProperty("Draw Speed")]
    public double BaseFeedRate
    {
        get => _baseFeedRate;
        set
        {
            _baseFeedRate = value;
            HandleChange();
        }
    }

    [JsonProperty] private bool _invertX;
    [JsonProperty] private bool _invertY = true;
    [JsonProperty] private bool _invertZ;
    [JsonProperty] private double _baseFeedRate = 3000;
    [JsonProperty] private string _axisX = "X";
    [JsonProperty] private string _axisY = "Y";
    [JsonProperty] private string _axisZ = "Z";
}