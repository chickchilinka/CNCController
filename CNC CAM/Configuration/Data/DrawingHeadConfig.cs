using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Data.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn), Name("Пишущая головка")]
public class DrawingHeadConfig:BaseConfig
{
    [JsonProperty, DBField]
    private double _headUp;
    [JsonProperty, DBField]
    private double _headDown;
    
    [ConfigProperty("Положение холостого хода")]
    public double HeadUp
    {
        get => _headUp;
        set
        {
            _headUp = value;
            HandleChange();
        }   
    }
    [ConfigProperty("Положение рабочего хода")]
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