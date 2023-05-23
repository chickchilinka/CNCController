using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CNC_CAM.Configuration.Attributes;
using CNC_CAM.Data.Attributes;
using DryIoc.ImTools;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration.Data;

[JsonObject(MemberSerialization.OptIn)]
public abstract class BaseConfig : INotifyPropertyChanged
{
    [JsonProperty, DBField(Priority = 2), DBPrimaryKey]
    private string _id;

    public string Id
    {
        get => _id;
        private set => _id = value;
    }

    [JsonProperty, DBField(Priority = 1)] private string _name;

    [ConfigProperty("Название", "Название настройки")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            HandleChange();
        }
    }

    [JsonProperty, DBField] private DateTime _created;

    [Attributes.ReadOnly]
    public DateTime Created
    {
        get => _created;
        private set => _created = value;
    }

    [JsonProperty, DBField] private DateTime _lastModified;

    [Attributes.ReadOnly]
    public DateTime LastModified
    {
        get => _lastModified;
        private set => _lastModified = value;
    }

    protected event Action OnChange;

    protected void HandleChange([CallerMemberName] string propertyName = null)
    {
        LastModified = DateTime.Now;
        OnPropertyChanged(propertyName);
    }

    public virtual void SubscribeToChange(Action callback)
    {
        OnChange += callback;
    }

    public virtual void UnsubscribeFromChange(Action callback)
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
            if (field.IsPrivate)
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
        OnChange?.Invoke();
    }
}