using CNC_CAM.Base;

namespace CNC_CAM.Configuration.Rule;

public class DeleteConfigRule:AbstractSignalRule<ConfigurationSignals.DeleteConfig>
{
    private ConfigurationStorage _configurationStorage;
    public DeleteConfigRule(ConfigurationStorage configurationStorage, SignalBus signalBus) : base(signalBus)
    {
        _configurationStorage = configurationStorage;
    }

    protected override void OnSignalFired(ConfigurationSignals.DeleteConfig signal)
    {
        var configType = signal.Config.GetType();
        if(_configurationStorage.GetAll(configType).Count == 1)
            return;
        var last = _configurationStorage.GetLast(configType);
        _configurationStorage.Remove(signal.Config);
        if(last == signal.Config)
            _configurationStorage.GetLast(configType);
    }
}