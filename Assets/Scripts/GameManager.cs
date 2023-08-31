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
        DataManager.Instance.LoadGameData();

        if (!isLoadSave || DataManager.Instance.data.worldData == null || DataManager.Instance.data == null)
        {
            Debug.Log("worldData Not find or No Save Mode, Generate New World");
            _worldGenerator.Generate();

        }
        else
        {
            if (DataManager.Instance.data.worldData._chunks.data.Count != 0)
            {
                Debug.Log("worldData Exist, Load Old World");
                _worldGenerator.LoadWorld(DataManager.Instance.data.worldData);
            }
            else
            {
                Debug.Log("worldData Get Damaged, Generate New World");
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