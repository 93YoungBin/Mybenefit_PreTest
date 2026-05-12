using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

    public static void Subscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (!_subscribers.ContainsKey(type))
        {
            _subscribers[type] = new List<object>();
        }

        _subscribers[type].Add(callback);
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (_subscribers.ContainsKey(type))
        {
            _subscribers[type].Remove(callback);
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type type = typeof(T);

        if (!_subscribers.ContainsKey(type))
        {
            return;
        }

        List<object> list = _subscribers[type];

        for (int i = list.Count - 1; i >= 0; i--)
        {
            (list[i] as Action<T>)?.Invoke(eventData);
        }
    }
}
