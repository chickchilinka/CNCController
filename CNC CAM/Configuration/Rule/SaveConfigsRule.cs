using System.Linq;
using CNC_CAM.Base;
using CNC_CAM.Configuration.Data;
using CNC_CAM.Data;
using CNC_CAM.Tools;
using CNC_CAM.Tools.Serialization;

namespace CNC_CAM.Configuration.Rule;

public class SaveConfigsRule : AbstractSignalRule<ConfigurationSignals.SaveConfigs>
{
    private ConfigurationStorage _configurationStorage;
    private DBService _dbService;

    public SaveConfigsRule(ConfigurationStorage configurationStorage, SignalBus signalBus,
        DBService dbService) : base(signalBus)
    {
        _configurationStorage = configurationStorage;
        _dbService = dbService;
    }

    protected override void OnSignalFired(ConfigurationSignals.SaveConfigs signal)
    {
        foreach (var collection in _configurationStorage.Configs.Values)
        {
            foreach (var config in collection)
            {
                _dbService.Save(config);
            }
        }

        var mainConfig = new Config("default");
        mainConfig.SetLastIds(_configurationStorage.LastConfigurations);
        _dbService.Save(mainConfig);
    }
}