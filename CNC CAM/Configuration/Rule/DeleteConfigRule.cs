using System.Linq;
using CNC_CAM.Base;
using CNC_CAM.Data;

namespace CNC_CAM.Configuration.Rule;

public class DeleteConfigRule:AbstractSignalRule<ConfigurationSignals.DeleteConfig>
{
    private ConfigurationStorage _configurationStorage;
    private DBService _dbService;
    public DeleteConfigRule(ConfigurationStorage configurationStorage, DBService dbService, SignalBus signalBus) : base(signalBus)
    {
        _configurationStorage = configurationStorage;
        _dbService = dbService;
    }

    protected override void OnSignalFired(ConfigurationSignals.DeleteConfig signal)
    {
        var configType = signal.Config.GetType();
        if(_configurationStorage.GetAll(configType).Count == 1)
            return;
        var last = _configurationStorage.GetLast(configType);
        _configurationStorage.Remove(signal.Config);
        _dbService.Remove(signal.Config);
        if(last == signal.Config)
            _configurationStorage.SetAsLast(_configurationStorage.GetAll(configType).FirstOrDefault());
    }
}