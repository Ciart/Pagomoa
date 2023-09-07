using System;
using System.Collections.Specialized;
using System.Drawing;
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

        if (!isLoadSave || DataManager.Instance.data == null) LoadSuccess = false;
        if(DataManager.Instance.data != null)
        {
            if (DataManager.Instance.data.worldData == null) LoadSuccess = false;
            if(AllBlockNullCheck()) LoadSuccess = false;
        }

        //Debug.Log("블럭모두없음?: " + AllBlockNullCheck());

        SaveManager.Instance.FreezePosition();

        if (isLoadSave && LoadSuccess)
        {
            try
            {
                _worldGenerator.LoadWorld(DataManager.Instance.data.worldData);
                SaveManager.Instance.LoadPosition();
            }
            catch
            {
                _worldGenerator.Generate();
                SaveManager.Instance.TagPosition();
            }
        }
        else
        {
            _worldGenerator.Generate();
            SaveManager.Instance.TagPosition(SaveManager.Instance.loadPositionDelayTime);
        }
    }
    bool AllBlockNullCheck()
    {
        if (DataManager.Instance.data == null) return true;
        bool allNullCheck = true;
        DicList<Vector2Int, Chunk> chunks = DataManager.Instance.data.worldData._chunks;
        if (chunks == null) return true;
        int dataSize = chunks.data.Count;
        //Debug.Log("수량: " + dataSize);
        for (int i = 0; i < dataSize; i++)
        {
            int brickSize = chunks.data[i].Value.bricks.Length;
            for (int j = 0; j < brickSize; j++)
            {
                if (chunks.data[i].Value.bricks[j].ground != null)
                    allNullCheck = false;
            }
        }
        return allNullCheck;
    }

    private void Update()
    {
        if (hasPowerGemEarth)
        {
            Debug.Log("데모 종료");
        }
    }
}
