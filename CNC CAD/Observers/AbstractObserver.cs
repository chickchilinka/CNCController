using System;
using System.Threading;

namespace CNC_CAD.Observers;

public abstract class AbstractObserver:IDisposable
{
    protected Thread ObservingThread;
    protected bool Stop;
    public void StartObserving()
    {
        ObservingThread = new Thread(() =>
        {
            while (true)
            {
                if(Stop)
                    return;
                Observe();
            }
        });
    }

    public void StopObserving()
    {
        Stop = true;
    }

    public void Dispose()
    {
        StopObserving();
    }

    protected abstract void Observe();
}