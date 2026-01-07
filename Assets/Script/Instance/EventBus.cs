// 文件: EventBus.cs
// 说明: 简单的事件总线实现，支持按事件类型注册/注销回调并发布事件。
using System;
using System.Collections.Generic;

public static class EventBus
{

    private static readonly Dictionary<System.Type, List<Delegate>> subscribers = new Dictionary<System.Type, List<Delegate>>();

    public static void Register<T>(System.Action<T> callback) where T : struct
    {
        var type = typeof(T);
        if (!subscribers.ContainsKey(type))
        {
            subscribers[type] = new List<Delegate>();
        }
        subscribers[type].Add(callback);
    }
    public static void Unregister<T>(System.Action<T> callback) where T : struct
    {
        var type = typeof(T);
        if (subscribers.ContainsKey(type))
        {
            subscribers[type].Remove(callback);
            if (subscribers[type].Count == 0)
            {
                subscribers.Remove(type);
            }
        }
    }
    public static void Publish<T>(T eventData) where T : struct
    {
        var type = typeof(T);
        if (subscribers.ContainsKey(type))
        {
            // Create a snapshot of the subscribers to avoid
            // "Collection was modified" exceptions if handlers
            // register/unregister during invocation.
            var subscriberSnapshot = subscribers[type].ToArray();
            foreach (var subscriber in subscriberSnapshot)
            {
                try
                {
                    ((System.Action<T>)subscriber)(eventData);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }
    }


}