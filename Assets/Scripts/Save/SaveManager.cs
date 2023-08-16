using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Worlds;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SaveManager : MonoBehaviour
{
    static GameObject container;
    static SaveManager instance;
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

        FreezePosition();
        Invoke("LoadPosition", 1.5f);
    }
    void AddManagingTargetWithTag(string tagName)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tagName);
        if (targets.Length == 0) return;
        foreach (GameObject target in targets)
            ManagingTargets.Add(target);
    }
    private void FreezePosition()
    {
        foreach(GameObject target in ManagingTargets)
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            target.GetComponent<Rigidbody2D>().Sleep();
        }
    }
    private void TagPosition()
    {
        foreach (GameObject target in ManagingTargets)
        {
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            target.GetComponent<Rigidbody2D>().WakeUp();
        }
    }
    private void LoadPosition()
    {
        TagPosition();

        if (DataManager.Instance.data.posData == null)
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

    void LoadMap()
    {

    }
    void WritePosData()
    {
        Dictionary<string, Vector3> posDataDictionary = new Dictionary<string, Vector3>();

        foreach (GameObject target in ManagingTargets)
            posDataDictionary.Add(target.name, target.transform.position);

        //Debug.Log(DataManager.Instance);
        //Debug.Log(DataManager.Instance.data);
        if(DataManager.Instance.data.posData == null)
        {
            Debug.Log("No Position Data before, Instantiate new Position Data");
            //return;
            DataManager.Instance.data.posData = new PositionData();
        }


        DataManager.Instance.data.posData.SetPositionDataFromDictionary(posDataDictionary);
    }
    void WriteMapData()
    {
        if(DataManager.Instance.data.worldData == null)
        {
            Debug.Log("No World Data before, Instantiate new World Data");
            DataManager.Instance.data.worldData = new WorldData();
        }
        DataManager.Instance.data.worldData.SetWorldDataFromWorld(WorldManager.instance.world);
    }
    private void OnApplicationQuit()
    {
        WritePosData();
        WriteMapData();

        DataManager.Instance.SaveGameData();
    }

}
