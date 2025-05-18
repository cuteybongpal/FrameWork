using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoad : IPool, IDependency 
{
    public T Load<T>(string key) where T : UnityEngine.Object;

}

public interface ILoadAsync : IPool, IDependency
{
    public UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object;
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object;
    public UniTask<T[]> LoadAllAsync<T>(string label) where T : UnityEngine.Object;
    public void LoadAllAsync<T>(string label, Action<T> callback = null) where T : UnityEngine.Object;
}

public class Loader : ILoad
{
    ResourceManager resourceManager;
    public void Init()
    {
        resourceManager = new ResourceManager();
    }

    public void Pool()
    {
        DIContainer.ReturnInstance(this);
        resourceManager = null;
    }
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        return resourceManager.Load<T>(key);
    }
}
public class AsyncLoader : ILoadAsync
{
    ResourceManager resourceManager;
    public void Init()
    {
        resourceManager = new ResourceManager();
    }

    public async UniTask<T[]> LoadAllAsync<T>(string label) where T : UnityEngine.Object
    {
        T[] loadedObjects = await resourceManager.LoadAllAsync<T>(label);

        if (loadedObjects == null)
        {
            Debug.Log($"{label} 라벨 값을 가진 오브젝트가 존재하지 않음");
            return null;
        }

        return loadedObjects;
    }

    public void LoadAllAsync<T>(string label, Action<T> callback = null) where T : UnityEngine.Object
    {
        resourceManager.LoadAllAsync<T>(label, callback);
    }

    public async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        T loadedObject = await resourceManager.LoadAsync<T>(key);
        if (loadedObject == null)
        {
            Debug.Log($"{key} 키값은 가진 오브젝트가 존재하지 않습니다.");
            return null;
        }
        return loadedObject;
    }

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        resourceManager.LoadAsync(key, callback);
    }

    public void Pool()
    {
        DIContainer.ReturnInstance(this);
        resourceManager = null;
    }
}