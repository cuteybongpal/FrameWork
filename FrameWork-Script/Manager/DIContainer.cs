using System;
using System.Collections.Generic;
using UnityEngine;

public static class DIContainer 
{
    static DIPool Pool = new DIPool();
    static Dictionary<Type, Func<IDependency>> BindedType = new Dictionary<Type, Func<IDependency>>();

    //��ü ����
    public static IDependency GetInstance<T>() where T : IDependency
    {
        if (!BindedType.ContainsKey(typeof(T)))
        {
            Debug.Log("DIContainer�� ��ϵ��� ���� ��ü�Դϴ�.");
            return default(T);
        }

        IDependency instance = Pool.GetInstance(typeof(T), BindedType[typeof(T)]);
        return instance;
    }
    //��ü �ݳ�
    public static void ReturnInstance<T>(T element) where T : class, IPool
    {
        Pool.ReturnInstance<T>(element);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="IKey">Ű : �������̽����� ��</typeparam>
    /// <typeparam name="ValueType">�� : Ű���� ��ӹ޴� �꿩����</typeparam>
    public static void Bind<IKey>(Func<IKey> func) where IKey : class, IDependency
    {
        BindedType.Add(typeof(IKey), func);
    }
}
