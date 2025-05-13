using System;
using UnityEngine;

public interface ILoad : IPool 
{
    public T Load<T>(string key) where T : UnityEngine.Object;

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
    public 
}
