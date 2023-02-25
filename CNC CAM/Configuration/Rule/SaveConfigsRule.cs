using CNC_CAM.Base;

namespace CNC_CAM.Configuration.Rule;

public class SaveConfigsRule:AbstractSignalRule<AppSignals.Exit>
{
    private CurrentConfiguration _currentConfiguration;
    public SaveConfigsRule(CurrentConfiguration currentConfiguration,SignalBus signalBus) : base(signalBus)
    {
        _currentConfiguration = currentConfiguration;
    }

    protected override void OnSignalFired(AppSignals.Exit signal)
    {
        _currentConfiguration.SaveConfig();
    }
}