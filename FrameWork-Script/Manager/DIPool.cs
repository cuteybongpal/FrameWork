using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DIPool
{
    Dictionary<Type, Queue<IPool>> PoolingDict = new Dictionary<Type, Queue<IPool>>();

    public IDependency GetInstance(Type type, Func<IDependency> func)
    { 
        if (type != typeof(IPool))
            return func.Invoke();

        if (!PoolingDict.ContainsKey(type))
            PoolingDict[type] = new Queue<IPool>();

        IPool dependency;

        if (PoolingDict[type].Count > 0 && type == typeof(IDependency))
            dependency = PoolingDict[type].Dequeue();
        else
            dependency = func.Invoke() as IPool;
        dependency.Init();
        return dependency as IDependency;

    }
    public void ReturnInstance<T>(T element)where T : class, IPool
    {
        if (PoolingDict.ContainsKey(typeof(T)))
            PoolingDict[typeof(T)].Enqueue(element);
        else
            PoolingDict[typeof(T)] = new Queue<IPool>();
    }
}