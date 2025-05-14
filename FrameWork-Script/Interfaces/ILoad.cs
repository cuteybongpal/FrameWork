using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoad : IPool 
{
    public T Load<T>(string key) where T : UnityEngine.Object;

}

public interface ILoadAsync : IPool
{
    public T LoadAsync<T>(string key) where T : UnityEngine.Object;

    public UniTask<List<T>> LoadAllAsync<T>(string key) where T : UnityEngine.Object;
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
    }}
