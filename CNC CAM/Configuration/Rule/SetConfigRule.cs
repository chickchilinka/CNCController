using CNC_CAM.Base;
using CNC_CAM.Tools.Serialization;

namespace CNC_CAM.Configuration.Rule;

public class SetConfigRule:AbstractSignalRule<ConfigurationSignals.SetConfig>
{
    private ConfigurationStorage _configurationStorage;
    public SetConfigRule(ConfigurationStorage configurationStorage, SignalBus signalBus) : base(signalBus)
    {
        _configurationStorage = configurationStorage;
    }

    protected override void OnSignalFired(ConfigurationSignals.SetConfig signal)
    {
        _configurationStorage.SetAsLast(signal.Config);
    }
}