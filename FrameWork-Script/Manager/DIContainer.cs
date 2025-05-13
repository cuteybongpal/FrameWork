using System;
using System.Collections.Generic;
using UnityEngine;

public static class DIContainer 
{
    static DIPool Pool = new DIPool();
    static Dictionary<Type, Func<IPool>> BindedType = new Dictionary<Type, Func<IPool>>();

    //��ü ����
    public static IPool GetInstance<T>() where T : IPool
    {
        if (!BindedType.ContainsKey(typeof(T)))
        {
            Debug.Log("DIContainer�� ��ϵ��� ���� ��ü�Դϴ�.");
            return default(T);
        }

        IPool instance = Pool.GetInstance(typeof(T), BindedType[typeof(T)]);
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
    public static void Bind<IKey>(Func<IPool> func) where IKey : class
    {
        BindedType.Add(typeof(IKey), func);
    }
}
