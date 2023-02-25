using System;
using CNC_CAM.Configuration.Attributes;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn)]
public abstract class BaseConfig
{
    [JsonProperty] private string _name;
    [ConfigProperty("Name", "Configuration name")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            HandleChange();
        } 
    }
    [JsonProperty, ReadOnly]
    public DateTime Created { get; private set; }
    [JsonProperty, ReadOnly]
    public DateTime LastModified { get; private set; }

    protected event Action OnChange;
    
    public BaseConfig(){}
    
    protected void HandleChange()
    {
        LastModified = DateTime.Now;
    }

    public virtual void SubscribeToChange(Action callback)
    {
        OnChange += callback;
    }
    public virtual void UnsubscribeToChange(Action callback)
    {
        OnChange -= callback;
    }

    public static TConfig Create<TConfig>(string name) where TConfig : BaseConfig, new()
    {
        var config = new TConfig();
        config._name = name;
        config.Created = DateTime.Now;
        config.LastModified = config.Created;
        return config;
    }

    public virtual TConfig Clone<TConfig>() where TConfig : BaseConfig, new()
    {
        var config = Create<TConfig>(Name);
        var fields = typeof(TConfig).GetFields();
        foreach (var field in fields)
        {
            if(field.IsPrivate)
                field.SetValue(config, field.GetValue(this));
        }
        return config;
    }
}