using UnityEngine;

public class GameController : MonoBehaviour 
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameController>();
            return _instance;
        }
    }

    public void StartWave()
    {
        
    }

    public void Damage()
    {
        
    }
}
