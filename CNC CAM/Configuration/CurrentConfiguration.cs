using System;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using DryIoc;

namespace CNC_CAM.Configuration;

public class CurrentConfiguration
{
    private ConfigurationStorage _configurationStorage;
    public event Action<Type> OnCurrentConfigChanged
    {
        add
        {
            _configurationStorage.OnCurrentConfigChanged += value;
        }
        remove
        {
            _configurationStorage.OnCurrentConfigChanged -= value;
        }
    }
    public CurrentConfiguration(ConfigurationStorage configurationStorage)
    {
        _configurationStorage = configurationStorage;
    }
    public TConfig Get<TConfig>() where TConfig : BaseConfig
    {
        return _configurationStorage.GetLast<TConfig>();
    }
    
    public BaseConfig Get(Type type)
    {
        return _configurationStorage.GetLast(type);
    }
}