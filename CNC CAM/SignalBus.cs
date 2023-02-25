using System;
using System.Collections.Generic;
using System.Linq;

namespace CNC_CAM;

public class SignalBus
{
    private class Subscription
    {
        public virtual Type Signal { get; private set; }
        public virtual Action<Object> Action { get; private set; }

        public Subscription(Type type, Action<Object> action)
        {
            Signal = type;
            Action = action;
        }
    }

    private class Subscription<T> : Subscription where T : class
    {
        public Subscription(Action<T> action) : base(typeof(T), obj => action((T)obj))
        {
        }
    }

    private List<Subscription> _subscriptions = new List<Subscription>();

    public void Fire<T>(T signal)
    {
        foreach (var subscription in _subscriptions)
        {
            if (subscription.Signal == typeof(T))
            {
                subscription.Action(signal);
            }
        }
    }

    public void Subscribe<T>(Action<T> action) where T : class
    {
        _subscriptions.Add(new Subscription<T>(action));
    }

    public void Unsubscribe<T>(Action<T> action) where T : class
    {
        var subscription = _subscriptions.FirstOrDefault(subscription => subscription.Action == action);
        if (subscription != null)
            _subscriptions.Remove(subscription);
    }
}