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
    public List<T> PoolingList = new List<T>();

    string key;
    public T Spawn()
    {
        if (PoolingList.Count != 0)
        {
            if (PoolingList[0] == null)
                PoolingList.Clear();
        }

        if (PoolingList.Count == 0)
        {
            GameObject go = resource.Load<GameObject>(key);
            T element = go.GetComponent<T>();
            objects.Add(element);
            return element;
        }
        else
        {
            T element = PoolingList[0];
            PoolingList.Remove(element);
            objects.Add(element);
            element.gameObject.SetActive(true);
            return element;
        }
    }
    public void DeSpawn(T element)
    {
        if (!objects.Remove(element))
        {
            Debug.Log("�ش� ��Ұ� objects�ؽ��¿� �������� �ʽ��ϴ�.");
            return;
        }
        element.gameObject.SetActive(false);
        element.transform.position = Vector3.zero;
        PoolingList.Add(element);

    }
    ResourceManager resource;
    public ManagedObject(string key)
    {
        resource = new ResourceManager();
        this.key = key;
    }
}