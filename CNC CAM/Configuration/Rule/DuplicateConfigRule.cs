using CNC_CAM.Base;

namespace CNC_CAM.Configuration.Rule;

public class DuplicateConfigRule:AbstractSignalRule<ConfigurationSignals.DuplicateConfig>
{
    private ConfigurationStorage _configurationStorage;
    public DuplicateConfigRule(ConfigurationStorage configurationStorage, SignalBus signalBus) : base(signalBus)
    {
        _configurationStorage = configurationStorage;
    }

    protected override void OnSignalFired(ConfigurationSignals.DuplicateConfig signal)
    {
        var newConfig = signal.Config.Clone();
        newConfig.Name += "_copy";
        _configurationStorage.RegisterConfig(newConfig);
    }
}