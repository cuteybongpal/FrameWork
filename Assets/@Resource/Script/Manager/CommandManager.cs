using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public T ExecuteCommand<T>(T command) where T : ICommand
    {
        return command.Execute<T>();
    }
}
