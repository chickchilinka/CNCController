using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CNC_CAM.Configuration.Attributes;
using DryIoc.ImTools;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn)]
public abstract class BaseConfig:INotifyPropertyChanged
{
    [JsonProperty] 
    public string Id { get; internal set; } 
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
    [JsonProperty, Attributes.ReadOnly]
    public DateTime Created { get; private set; }
    [JsonProperty, Attributes.ReadOnly]
    public DateTime LastModified { get; private set; }

    protected event Action OnChange;
    
    public BaseConfig(){}
    
    protected void HandleChange()
    {
        LastModified = DateTime.Now;
        OnPropertyChanged();
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
        config.Id = config.GetHashCode().ToString();
        config._name = name;
        config.Created = DateTime.Now;
        config.LastModified = config.Created;
        return config;
    }

    public virtual BaseConfig Clone()
    {
        var config = Activator.CreateInstance(GetType()) as BaseConfig;
        var fields = GetType().GetFields();
        foreach (var field in fields)
        {
            if(field.IsPrivate)
                field.SetValue(config, field.GetValue(this));
        }
        config.Id = config.GetHashCode().ToString();
        config._name = _name;
        config.Created = DateTime.Now;
        config.LastModified = config.Created;
        return config;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}