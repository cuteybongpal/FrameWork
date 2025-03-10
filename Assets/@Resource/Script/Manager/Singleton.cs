using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance {  get { return instance; } protected set { instance = value; } }
}
