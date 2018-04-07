using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner _instance;

    public static Spawner Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Spawner>();
            return _instance;
        }
    }
    
    public void SpawnBot()
    {
        // берем модель из генетики 
    }
}
