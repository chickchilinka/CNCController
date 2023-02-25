using CNC_CAM.Configuration.Data;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;

namespace CNC_CAM.Configuration;

public class CurrentConfiguration
{
    private SerializationService _serializationService;
    private ConfigurationStorage _configurationStorage;
    public CurrentConfiguration(SerializationService serializationService)
    {
        _serializationService = serializationService;
        LoadConfigs();
    }
    public TConfig GetCurrentConfig<TConfig>() where TConfig : BaseConfig
    {
        return _configurationStorage.GetLast<TConfig>();
    }

    public void SaveConfig()
    {
        _serializationService.Serialize(Const.Paths.ConfigurationsPath, Const.Paths.LastConfigsFilename, _configurationStorage);
    }
    protected void LoadConfigs()
    {
        _configurationStorage = _serializationService.Deserialize(
            Const.Paths.ConfigurationsPath, Const.Paths.LastConfigsFilename, 
            new ConfigurationStorage());
        if (_configurationStorage.LastConfigurations.Keys.Count == 0)
        {
            foreach (var config in Const.Configs.DefaultConfigs)
            {
                _configurationStorage.RegisterConfig(config);
                _configurationStorage.SetAsLast(config);
            }
        }
    }
}