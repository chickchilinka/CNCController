using System;
using System.Collections.Generic;
using System.Linq;
using CNC_CAM.Configuration.Data;
using Newtonsoft.Json;

namespace CNC_CAM.Configuration;


public class ConfigurationStorage
{
    [JsonProperty]
    internal Dictionary<Type, List<BaseConfig>> Configs = new();
    [JsonProperty] 
    internal Dictionary<Type, string> LastConfigurations = new();

    public void RegisterConfig<TConfig>(TConfig config) where TConfig : BaseConfig
    {
        var type = config.GetType();
        if(!Configs.ContainsKey(type))
            Configs.Add(type, new List<BaseConfig>());
        Configs[type].Add(config);
    }

    public void SetAsLast<TConfig>(TConfig config) where TConfig:BaseConfig
    {
        var type = config.GetType();
        if (!LastConfigurations.ContainsKey(type))
        {
            LastConfigurations.Add(type, config.Name);
            return;
        }
        LastConfigurations[type] = config.Name;
    }

    public List<TConfig> GetAll<TConfig>() where TConfig : BaseConfig
    {
        if (Configs.TryGetValue(typeof(TConfig), out var configs))
            return configs.Select(config=>config as TConfig).ToList();
        
        return new List<TConfig>();
    }

    public TConfig Get<TConfig>(string name) where TConfig : BaseConfig
    {
        if (Configs.TryGetValue(typeof(TConfig), out var list))
        {
            return list.FirstOrDefault(config => config.Name.Equals(name)) as TConfig;
        }
        return null;
    }
    public TConfig GetLast<TConfig>() where TConfig : BaseConfig
    {
        var type = typeof(TConfig);
        if (LastConfigurations.TryGetValue(type, out var name))
            return Get<TConfig>(name);
        if (Configs.ContainsKey(type) && Configs[type].Count > 0)
        {
            SetAsLast(Configs[type].First() as TConfig);
            return GetLast<TConfig>();
        }
        return null;
    }
    
}