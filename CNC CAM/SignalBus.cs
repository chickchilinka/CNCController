using System;
using System.Collections.Generic;

namespace CNC_CAM;

public static class SignalBus
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
    private class Subscription<T>:Subscription where T:class
    {
        public Subscription(Action<T> action) : base(typeof(T), obj => action((T)obj))
        {
        }
    }

    private static List<Subscription> _subscriptions = new List<Subscription>();

    public static void Fire<T>(T signal)
    {
        foreach (var subscription in _subscriptions)
        {
            if (subscription.Signal == typeof(T))
            {
                subscription.Action(signal);
            }
        }
    }
    
    public static void Subscribe<T>(Action<T> action) where T:class
    {
        _subscriptions.Add(new Subscription<T>(action));
    }
}