using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager
{
    public static Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();


    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (resources.ContainsKey(key))
        {
            return resources[key] as T;
        }
        Debug.Log($"{key} 키값을 가진 오브젝트가 Addressabel에 없습니다.");
        return null;
    }

    public (string[],T[]) LoadAll<T>(Define.AddressableLabelGroup labelGroup) where T : UnityEngine.Object
    {
        List<T> objs = new List<T>();
        List<string> keys = new List<string>();
        string[] dictKeys = resources.Keys.ToArray<string>();
        foreach (string key in resources.Keys.ToArray())
        {
            if (key.StartsWith(labelGroup.ToString()+"/"))
            {
                objs.Add(Load<T>(key));
                keys.Add(key);
            }
        }
        return (keys.ToArray<string>(),objs.ToArray<T>());
    }

    public async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        if (resources.ContainsKey(key))
            return resources[key] as T;

        AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);

        //비동기 작업이 끝날때까지 대기함
        await op.Task;
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            T obj = op.Result;
            resources.Add(key, obj as UnityEngine.Object);

            return obj;
        }
        else
        {
            Debug.LogError($"Addressable 동기 로딩 실패 키값 : {key}");
            return null;
        }
    }

    public void LoadAsync<T>(string key, Action<T> action = null) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);
        op.Completed += (loadedObject) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                T obj = op.Result;
                resources.Add(key, obj as UnityEngine.Object);
                action?.Invoke(obj);
            }
            else
                Debug.LogError($"Addressable 비동기 로딩 실패 키값 : {key}");
        };
    }

    public async UniTask<T[]> LoadAllAsync<T>(string label) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> op = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        await op.Task;
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            T[] objs = new T[op.Result.Count];
            int index = 0;
            foreach (IResourceLocation item in op.Result)
            {
                T obj = await LoadAsync<T>(item.PrimaryKey);
                objs[index] = obj;
                index++;
            }
            return objs;
        }
        else
            Debug.Log($"LoadAllAsync 실패 라벨값 {label}");
        return null;
        
    }
    public void LoadAllAsync<T>(string label, Action<UnityEngine.Object> callback) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> op = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        op.Completed += (loadedObject) =>
        {
            IList<IResourceLocation> keys = loadedObject.Result;
            foreach(IResourceLocation item in keys)
            {
                string key = item.PrimaryKey;
                LoadAsync<T>(key, callback);
            }
        };
    }

    public void UnLoad<T>(string key) where T : UnityEngine.Object
    {
        T obj = Load<T>(key);
        Addressables.Release<T>(obj);
        resources.Remove(key);
    }
    public void UnLoadAll<T>(Define.AddressableLabelGroup label) where T : UnityEngine.Object
    {
        (string[] ,T[]) objs = LoadAll<T>(label);

        for (int i = 0; i < objs.Item1.Length; i++)
        {
            Addressables.Release(objs.Item2[i]);
            resources.Remove(objs.Item1[i]);
        }
    }
}

