using System;
using UnityEngine;
using Worlds;

public class GameManager : MonoBehaviour
{
    public bool isLoadSave;

    public bool hasPowerGemEarth;
    
    private static GameManager _instance;
    
    private WorldManager _worldManager;
    
    private WorldGenerator _worldGenerator;
    
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
        
        _worldManager = WorldManager.instance;
        _worldGenerator = _worldManager.GetComponent<WorldGenerator>();
    }

    private void Start()
    {
        bool LoadSuccess = DataManager.Instance.LoadGameData();

        if (!isLoadSave || DataManager.Instance.data.worldData == null || DataManager.Instance.data == null) LoadSuccess = false;

        if (!LoadSuccess)
            _worldGenerator.Generate();
        else
        {
            try
            {
                _worldGenerator.LoadWorld(DataManager.Instance.data.worldData);
            }
            catch
            {
                _worldGenerator.Generate();
            }
        }
    }

    private void Update()
    {
        if (hasPowerGemEarth)
        {
            Debug.Log("데모 종료");
        }
    }
}