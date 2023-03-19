using CNC_CAM.Base;

namespace CNC_CAM.Configuration.Rule;

public class SetConfigRule:AbstractSignalRule<ConfigurationSignals.SetConfig>
{
    private CurrentConfiguration _currentConfiguration;
    public SetConfigRule(CurrentConfiguration configuration, SignalBus signalBus) : base(signalBus)
    {
        _currentConfiguration = configuration;
    }

    protected override void OnSignalFired(ConfigurationSignals.SetConfig signal)
    {
        _currentConfiguration.SetCurrentConfig(signal.Config);
    }
}