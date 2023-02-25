using System;

namespace CNC_CAM.Base;

public abstract class AbstractSignalRule<TSignal>:IDisposable where TSignal:class
{
    protected SignalBus SignalBus;
    public AbstractSignalRule(SignalBus signalBus)
    {
        SignalBus = signalBus;
        signalBus.Subscribe<TSignal>(OnSignalFired);
    }

    protected abstract void OnSignalFired(TSignal signal);
    public void Dispose()
    {
        
    }
}