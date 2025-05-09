using UnityEngine;

public class Initializer : MonoBehaviour
{
    ServiceLocator locater;
    ResourceManager resourceManager;
    private async void Awake()
    {
        locater = new ServiceLocator();
        resourceManager = new ResourceManager();

        await resourceManager.LoadAllAsync<GameObject>("�󺧰� �����ÿ�");
        locater.Set<ObjectManager>(new ObjectManager());
    }

}
