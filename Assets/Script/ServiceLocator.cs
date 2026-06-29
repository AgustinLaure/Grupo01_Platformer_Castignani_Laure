using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void AddService<T>(T service) where T : class
    {
        var type = typeof(T);

        if (!_services.TryAdd(type, service))
        {
            Debug.LogWarning("Ya esta suscripto: " + type);
        }
    }

    public static void RemoveService<T>(T service) where T : class
    {
        var type = typeof(T);

        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
        else
        {
            Debug.LogWarning("No existe el servicio: " + type);
        }
    }

    public static T GetService<T>() where T : class
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }

        Debug.LogWarning("No esta suscripto: " + type);
        return null;
    }

    public static void ClearServices() => _services.Clear();
}

