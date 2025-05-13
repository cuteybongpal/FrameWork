using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager
{
    Dictionary<Type, HashSet<IPool>> PoolingDict = new Dictionary<Type, HashSet<IPool>>();


    public T Get<T>() where T : class
    {
        Type type = typeof(T);

        if (!PoolingDict.ContainsKey(type))
            PoolingDict[type] = new HashSet<IPool>();

        IPool pool = PoolingDict[type].First();
        PoolingDict[type].Remove(pool);

        return pool as T;

    }
    public void StopUse<T>(T element)
    {

    }
}