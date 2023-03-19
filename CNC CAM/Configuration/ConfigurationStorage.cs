using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CNC_CAM.Configuration.Data;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration;


public class ConfigurationStorage
{
    [JsonProperty]
    internal Dictionary<Type, ObservableCollection<BaseConfig>> Configs = new();
    [JsonProperty] 
    internal Dictionary<Type, string> LastConfigurations = new();

    public void RegisterConfig<TConfig>(TConfig config) where TConfig : BaseConfig
    {
        var type = config.GetType();
        if(!Configs.ContainsKey(type))
            Configs.Add(type, new ObservableCollection<BaseConfig>());
        Configs[type].Add(config);
    }

    public void SetAsLast<TConfig>(TConfig config) where TConfig:BaseConfig
    {
        var type = config.GetType();
        if (!LastConfigurations.ContainsKey(type))
        {
            LastConfigurations.Add(type, config.Id);
            return;
        }
        LastConfigurations[type] = config.Id;
    }

    public ObservableCollection<BaseConfig> GetAll(Type type) 
    {
        if (Configs.TryGetValue(type, out var collection))
            return collection;
        return null;
    }
    public ObservableCollection<BaseConfig> GetAll<TConfig>() where TConfig : BaseConfig
    {
        return GetAll(typeof(TConfig));
    }

    public BaseConfig Get(Type type, string id)
    {
        if (Configs.TryGetValue(type, out var list))
        {
            return list.FirstOrDefault(config => config.Id.Equals(id));
        }
        return null;
    }
    public TConfig Get<TConfig>(string id) where TConfig : BaseConfig
    {
        return Get(typeof(TConfig), id) as TConfig;
    }

    public void Remove<TConfig>(TConfig config) where TConfig : BaseConfig
    {
        if (Configs.TryGetValue(config.GetType(), out var list))
        {
            list.Remove(config);
            if (LastConfigurations.ContainsValue(config.Id))
                LastConfigurations.Remove(config.GetType());
        }
    }

    public BaseConfig GetLast(Type type)
    {
        if (LastConfigurations.TryGetValue(type, out var name))
            return Get(type, name);
        if (Configs.ContainsKey(type) && Configs[type].Count > 0)
        {
            SetAsLast(Configs[type].First());
            return GetLast(type);
        }
        return null;
    }
    public TConfig GetLast<TConfig>() where TConfig : BaseConfig
    {
        var type = typeof(TConfig);
        return GetLast(type) as TConfig;
    }

}