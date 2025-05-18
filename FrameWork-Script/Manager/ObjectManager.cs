using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    GameObjectPool pool = new GameObjectPool();
    public List<Object> SpawnedObject = new List<Object>();
    public T Spawn<T>(string key) where T : MonoBehaviour, IPool
    {
        T spawnedObject = pool.Spawn<T>(key);
        SpawnedObject.Add(spawnedObject);
        return spawnedObject;
    }
    public void DeSpawn<T>(T element) where T : MonoBehaviour, IPool
    {
        pool.DeSpawn<T>(element);
        SpawnedObject.Remove(element);
    }
}