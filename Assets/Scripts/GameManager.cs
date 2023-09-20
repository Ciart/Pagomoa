using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isLoadSave;

    public bool hasPowerGemEarth;
    
    private static GameManager _instance;

    private PlayerController _player;
    
    public static GameManager instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }

            return _instance;
        }
    }
    
    public PlayerController player
    {
        get
        {
            if (_player is null)
            {
                _player = (PlayerController)FindObjectOfType(typeof(PlayerController));
            }

            return _player;
        }
    }
    
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SaveManager saveManager = SaveManager.Instance;
        bool mapLoad = saveManager.LoadMap();
        if (mapLoad)
        {
            saveManager.LoadPosition();
            saveManager.LoadItem();
            saveManager.LoadArtifactItem();
            saveManager.LoadQuickSlot();
        }
        else
            saveManager.TagPosition(saveManager.loadPositionDelayTime);
    }


    private void Update()
    {
        if (hasPowerGemEarth)
        {
            Debug.Log("데모 종료");
        }
    }
}
