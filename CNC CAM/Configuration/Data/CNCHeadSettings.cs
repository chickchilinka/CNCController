using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn)]
public class CNCHeadSettings:BaseConfig
{
    [JsonProperty]
    private double _headUp;
    [JsonProperty]
    private double _headDown;
    
    [ConfigProperty("Head Idle Position")]
    public double HeadUp
    {
        get => _headUp;
        set
        {
            _headUp = value;
            HandleChange();
        }   
    }
    [ConfigProperty("Head Work Position")]
    public double HeadDown
    {
        get => _headDown;
        set
        {
            _headDown = value;
            HandleChange();
        }
    }
    
}