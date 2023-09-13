using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Worlds;

public class SaveManager : MonoBehaviour
{
    static GameObject container;
    static SaveManager instance;

    public float loadPositionDelayTime = 1.5f;
    public bool LoadComplete = false;

    private WorldManager _worldManager;

    private WorldGenerator _worldGenerator;

    public static SaveManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "Save Manager";
                instance = container.AddComponent(typeof(SaveManager)) as SaveManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
    List<GameObject> ManagingTargets = new List<GameObject>();
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Title") return;

        if (!container)
        {
            container = gameObject;
            instance = this;
            DontDestroyOnLoad(container);
        }

        //DataManager.Instance.LoadGameData();

        AddManagingTargetWithTag("Player");
        AddManagingTargetWithTag("Monster");
        AddManagingTargetWithTag("NPC");

    }
    
    bool AllBlockNullCheck()
    {
        if (DataManager.Instance.data == null) return true;
        bool allNullCheck = true;
        DicList<Vector2Int, Chunk> chunks = DataManager.Instance.data.worldData._chunks;
        if (chunks == null) return true;
        int dataSize = chunks.data.Count;
        //Debug.Log("����: " + dataSize);
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
    void AddManagingTargetWithTag(string tagName)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tagName);
        if (targets.Length == 0) return;
        foreach (GameObject target in targets)
        {
            if (target.activeSelf)
                ManagingTargets.Add(target);
        }
    }
    public void FreezePosition()
    {
        foreach (GameObject target in ManagingTargets)
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            target.GetComponent<Rigidbody2D>().Sleep();
        }
    }
    public void TagPosition(float time = 0)
    {
        Invoke("_TagPosition", time);
    }
    private void _TagPosition()
    {
        LoadComplete = true;
        foreach (GameObject target in ManagingTargets)
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            target.GetComponent<Rigidbody2D>().WakeUp();
        }
    }
    public void LoadPosition()
    {
        Invoke("_LoadPosition", loadPositionDelayTime);
    }
    private void _LoadPosition()
    {
        TagPosition();

        if (DataManager.Instance.data.posData == null)
            return;
        if (DataManager.Instance.data.posData.positionData == null)
            return;

        Dictionary<string, Vector3> posDataDictionary = ListDictionaryConverter.ToDictionary(DataManager.Instance.data.posData.positionData);
        if (posDataDictionary.Count != 0)
        {
            foreach (string key in posDataDictionary.Keys)
            {
                GameObject target = GameObject.Find(key);
                if (target)
                    target.transform.position = posDataDictionary[key];
            }
        }
    }

    public bool LoadMap()
    {
        bool LoadSuccess = DataManager.Instance.LoadGameData();

        if (!GameManager.instance.isLoadSave || DataManager.Instance.data == null) LoadSuccess = false;
        if (DataManager.Instance.data != null)
        {
            if (DataManager.Instance.data.worldData == null) LoadSuccess = false;
            if (AllBlockNullCheck()) LoadSuccess = false;
        }

        //Debug.Log("����ξ���?: " + AllBlockNullCheck());

        FreezePosition();

        if (GameManager.instance.isLoadSave && LoadSuccess)
        {
            try
            {
                WorldManager.instance.GetComponent<WorldGenerator>().LoadWorld(DataManager.Instance.data.worldData);
                return true;
            }
            catch
            {
                WorldManager.instance.GetComponent<WorldGenerator>().Generate();
                return false;
            }
        }
        else
        {
            WorldManager.instance.GetComponent<WorldGenerator>().Generate();
            return false;
        }
    }

    public void LoadItem()
    {
        DataManager dataManager = DataManager.Instance;
        if (dataManager.data.itemData == null) return;

        if (dataManager.data.itemData.items != null)
        {
            InventoryDB.Instance.items = dataManager.data.itemData.items.ToList();
            InventoryDB.Instance.Gold = dataManager.data.itemData.gold;
        }
        else
            Debug.Log("Item Data is Nothing");
    }
    public void LoadOption()
    {
        if (DataManager.Instance.data.optionData != null)
        {
            OptionDB.instance.scale = DataManager.Instance.data.optionData.scale;
            OptionDB.instance.audioValue = DataManager.Instance.data.optionData.audioValue;
        }
    }
    public void LoadArtifactItem()
    {
        if(DataManager.Instance.data.artifactData != null && ArtifactSlotDB.Instance != null)
            if(DataManager.Instance.data.artifactData.artifacts != null)
                ArtifactSlotDB.Instance.Artifact = DataManager.Instance.data.artifactData.artifacts.ToList();
    }   
    void WritePosData()
    {
        Dictionary<string, Vector3> posDataDictionary = new Dictionary<string, Vector3>();

        foreach (GameObject target in ManagingTargets)
            posDataDictionary.Add(target.name, target.transform.position);

        InitData();

        DataManager.Instance.data.posData.SetPositionDataFromDictionary(posDataDictionary);
    }
    void WriteMapData()
    {
        InitData();
        DataManager.Instance.data.worldData.SetWorldDataFromWorld(WorldManager.instance.world);
    }

    public void WriteIntroData(bool arg)
    {
        InitData();
        DataManager.Instance.data.introData.isFirstStart = arg;
    }
    void WriteItemData()
    {
        InitData();
        DataManager.Instance.data.itemData.SetItemDataFromInventoryDB(InventoryDB.Instance);
    }
    public void WriteOptionData()
    {
        InitData();
        DataManager.Instance.data.optionData.SetOptionDataFromOptionDB(OptionDB.instance);
    }
    void WriteArtifactData()
    {
        InitData();
        DataManager.Instance.data.artifactData.SetArtifactDataFromArtifactSlotDB(ArtifactSlotDB.Instance);
    }

    public void InitData()
    {
        if (DataManager.Instance.data == null)
            DataManager.Instance.data = new GameData();

        if (DataManager.Instance.data.posData == null)
        {
            Debug.Log("No Position Data before, Instantiate new Position Data");
            DataManager.Instance.data.posData = new PositionData();
        }
        if (DataManager.Instance.data.worldData == null)
        {
            Debug.Log("No World Data before, Instantiate new World Data");
            DataManager.Instance.data.worldData = new WorldData();
        }
        if (DataManager.Instance.data.introData == null)
        {
            Debug.Log("No Intro Data before, Instantiate new Intro Data");
            DataManager.Instance.data.introData = new IntroData();
        }
        if (DataManager.Instance.data.itemData == null)
        {
            Debug.Log("No Item Data before, Instantiate new Item Data");
            DataManager.Instance.data.itemData = new ItemData();
        }
        if (DataManager.Instance.data.optionData == null)
        {
            Debug.Log("No Option Data before, Instantiate new Option Data");
            DataManager.Instance.data.optionData = new OptionData();
        }
        if (DataManager.Instance.data.artifactData == null)
        {
            Debug.Log("No Artifact Data before, Instantiate new Artifact Data");
            DataManager.Instance.data.artifactData = new ArtifactData();
        }
    }

    private void OnApplicationQuit()
    {
        if (!LoadComplete) return;
        WritePosData();
        WriteMapData();
        WriteItemData();
        WriteOptionData();
        WriteArtifactData();
        DataManager.Instance.SaveGameData();
    }

}
