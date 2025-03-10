using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ObjectManager
{

}

public class ManagedObject<T> where T : Object
{
    public HashSet<T> objects = new HashSet<T>();
    public List<T> list = new List<T>();

}