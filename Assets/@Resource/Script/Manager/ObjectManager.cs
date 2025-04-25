using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class ObjectManager
{

}

public class ManagedObject<T> where T : MonoBehaviour
{
    //현재 사용되는 오브젝트들
    public HashSet<T> objects = new HashSet<T>();
    //현재 사용되지 않는 오브젝트들
    public List<T> Poolinglist = new List<T>();
    ResourceManager resourceManager;
    string key;
    /// <summary>
    /// 관리해주는 오브젝트를 스폰해준다.
    /// </summary>
    /// <returns>오브젝트</returns>
    public virtual T Spawn()
    {
        //풀링 리스트에서 요소를 꺼내서 반환
        for (int i = 0; i < Poolinglist.Count; i++)
        {
            T element = Poolinglist[i];
            if (element == null)
                continue;
            element.gameObject.SetActive(true);
            Poolinglist.RemoveAt(i);
            objects.Add(element);
            return element;
        }
        //없다면 리소스 매니저에서 로드를 해 반환해준다.
        GameObject go = resourceManager.Load<GameObject>(key);
        T _element = go.GetComponent<T>();
        _element.gameObject.SetActive(true);
        objects.Add(_element);
        return _element;
    }
    /// <summary>
    /// 관리하는 오브젝트중 하나를 디스폰함
    /// </summary>
    /// <param name="element"></param>
    public void Despawn(T element)
    {
        objects.Remove(element);
        element.gameObject.SetActive(false);
        Poolinglist.Add(element);
    }

    public ManagedObject(string key)
    {
        resourceManager = new ResourceManager();
        this.key = key;
    }

}