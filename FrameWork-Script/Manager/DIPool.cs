using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DIPool
{
    Dictionary<Type, Queue<IPool>> PoolingDict = new Dictionary<Type, Queue<IPool>>();

    public IPool GetInstance(Type type, Func<IPool> func)
    { 
        if (!PoolingDict.ContainsKey(type))
            PoolingDict[type] = new Queue<IPool>();

        IPool pool;

        if (PoolingDict[type].Count > 0)
            pool = PoolingDict[type].Dequeue();
        else
            pool = func.Invoke();

        return pool;

    }
    public void ReturnInstance<T>(T element)where T : class, IPool
    {
        if (PoolingDict.ContainsKey(typeof(T)))
            PoolingDict[typeof(T)].Enqueue(element);
        else
            PoolingDict[typeof(T)] = new Queue<IPool>();
    }
}