using System;
using System.Collections.Generic;
using UnityEngine;

public static class DIContainer 
{
    static DIPool Pool = new DIPool();
    static Dictionary<Type, Func<IPool>> BindedType = new Dictionary<Type, Func<IPool>>();

    //객체 얻어옴
    public static IPool GetInstance<T>() where T : IPool
    {
        if (!BindedType.ContainsKey(typeof(T)))
        {
            Debug.Log("DIContainer에 등록되지 않은 객체입니다.");
            return default(T);
        }

        IPool instance = Pool.GetInstance(typeof(T), BindedType[typeof(T)]);
        return instance;
    }
    //객체 반납
    public static void ReturnInstance<T>(T element) where T : class, IPool
    {
        Pool.ReturnInstance<T>(element);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="IKey">키 : 인터페이스여야 함</typeparam>
    /// <typeparam name="ValueType">값 : 키값을 상속받는 얘여야함</typeparam>
    public static void Bind<IKey>(Func<IPool> func) where IKey : class
    {
        BindedType.Add(typeof(IKey), func);
    }
}
