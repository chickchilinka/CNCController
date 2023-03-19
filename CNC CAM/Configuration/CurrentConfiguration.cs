using System;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;
using DryIoc;

namespace CNC_CAM.Configuration;

public class CurrentConfiguration
{
    private SerializationService _serializationService;
    private ConfigurationStorage _configurationStorage;
    public event Action<Type> OnCurrentConfigChanged = delegate {};
    public CurrentConfiguration(ConfigurationStorage configurationStorage, SerializationService serializationService)
    {
        _configurationStorage = configurationStorage;
        _serializationService = serializationService;
    }
    public TConfig GetCurrentConfig<TConfig>() where TConfig : BaseConfig
    {
        return _configurationStorage.GetLast<TConfig>();
    }
    
    public BaseConfig GetCurrentConfig(Type type)
    {
        return _configurationStorage.GetLast(type);
    }

    public void SetCurrentConfig(BaseConfig config)
    {
        _configurationStorage.SetAsLast(config);
        OnCurrentConfigChanged?.Invoke(config.GetType());
    }
    

    public void SaveConfig()
    {
        _serializationService.Serialize(Const.Paths.ConfigurationsPath, Const.Paths.LastConfigsFilename, _configurationStorage);
    }
    
}