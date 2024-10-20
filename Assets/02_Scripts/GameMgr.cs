using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
