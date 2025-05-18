using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    Dictionary<Type, Queue<IPool>> PoolingDict = new Dictionary<Type, Queue<IPool>>();

    public T Spawn<T>(string key) where T : MonoBehaviour, IPool
    {
        Type type = typeof(T);

        if (!PoolingDict.ContainsKey(type))
            PoolingDict[type] = new Queue<IPool>();

        IPool pool;

        if (PoolingDict[type].Count > 0)
            pool = PoolingDict[type].Dequeue();
        else
        {
            ILoad loading = DIContainer.GetInstance<ILoad>() as ILoad;
            pool = loading.Load<GameObject>(key).GetComponent<T>();
            loading.Pool();
        }

        return pool as T;

    }
    public void DeSpawn<T>(T element) where T : MonoBehaviour, IPool
    {
        if (PoolingDict.ContainsKey(typeof(T)))
            PoolingDict[typeof(T)].Enqueue(element);
        else
            PoolingDict[typeof(T)] = new Queue<IPool>();
    }

}
