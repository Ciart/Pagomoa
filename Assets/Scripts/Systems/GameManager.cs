using System;
using System.Collections.Specialized;
using System.Drawing;
using Entities.Players;
using UnityEngine;
using Worlds;
using PlayerController = Entities.Players.PlayerController;

public class GameManager : MonoBehaviour
{
    public bool isLoadSave;

    public bool hasPowerGemEarth;

    public GameObject Player;
    
    private static GameManager _instance;

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
    
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindObjectOfType<PlayerSpawnPoint>().Spawn();
        
        SaveManager saveManager = SaveManager.Instance;
        bool mapLoad = saveManager.LoadMap();
        if (mapLoad)
        {
            saveManager.LoadPosition();
            saveManager.LoadItem();
            saveManager.LoadArtifactItem();
            saveManager.LoadQuickSlot();
            saveManager.LoadPlayerCurrentStatusData();
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
