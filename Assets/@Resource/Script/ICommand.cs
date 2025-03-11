using UnityEngine;
public interface ICommand
{
    public T Execute<T>();
}