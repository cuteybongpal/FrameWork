using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.AddressableAssets.Settings;
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

    public async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);

        //비동기 작업이 끝날때까지 대기함
        await op.Task;

        try
        {
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
        finally
        {
            Addressables.Release(op);
        }
    }

    public void LoadAsync<T>(string key, Action<T> action = null) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);
        op.Completed += (bojca) =>
        {
            try
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    T obj = op.Result;
                    resources.Add(key, obj as UnityEngine.Object);
                    action?.Invoke(obj);
                }
                else
                    Debug.LogError($"Addressable 비동기 로딩 실패 키값 : {key}");
            }

            finally
            {
                Addressables.Release(op);
            }
        };
    }

    public async UniTask LoadAllAsync<T>(string label) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> op = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        await op.Task;

        try
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                foreach(IResourceLocation item in op.Result)
                {
                    await LoadAsync<T>(item.PrimaryKey);
                }
            }
            else
            {
                Debug.Log($"LoadAllAsync 실패 라벨값 {label}");
            }
        }
        finally
        {
            Addressables.Release(op);
        }
    }
    public void LoadAllAsync<T>(string label) where T : UnityEngine.Object
    {

    }
}

