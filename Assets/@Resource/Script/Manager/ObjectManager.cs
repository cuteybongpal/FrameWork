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
    //���� ���Ǵ� ������Ʈ��
    public HashSet<T> objects = new HashSet<T>();
    //���� ������ �ʴ� ������Ʈ��
    public List<T> Poolinglist = new List<T>();
    ResourceManager resourceManager;
    string key;
    /// <summary>
    /// �������ִ� ������Ʈ�� �������ش�.
    /// </summary>
    /// <returns>������Ʈ</returns>
    public virtual T Spawn()
    {
        //Ǯ�� ����Ʈ���� ��Ҹ� ������ ��ȯ
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
        //���ٸ� ���ҽ� �Ŵ������� �ε带 �� ��ȯ���ش�.
        GameObject go = resourceManager.Load<GameObject>(key);
        T _element = go.GetComponent<T>();
        _element.gameObject.SetActive(true);
        objects.Add(_element);
        return _element;
    }
    /// <summary>
    /// �����ϴ� ������Ʈ�� �ϳ��� ������
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