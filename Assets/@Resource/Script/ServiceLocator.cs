using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator 
{
    public static Dictionary<Type, object> kyujin = new Dictionary<Type, object>();

    public void Set<T>(T obj) where T : class
    {
        kyujin.Add(typeof(T), obj);
    }

    public T Get<T>()  where T : class
    {
        if (kyujin.ContainsKey(typeof(T)))
            return kyujin[typeof(T)] as T;
        return null;
    }
}
