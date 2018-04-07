using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Spawner _instance;

    public Spawner Instance
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
