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

        DataManager.Instance.LoadGameData();

        AddManagingTargetWithTag("Player");
        AddManagingTargetWithTag("Monster");
        AddManagingTargetWithTag("NPC");

        Invoke("LoadPosition", 0.5f); 
    }
    void AddManagingTargetWithTag(string tagName)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tagName);
        if (targets.Length == 0) return;
        foreach (GameObject target in targets)
            ManagingTargets.Add(target);
    }

    private void LoadPosition()
    {
        Dictionary<string, Vector3> posDataDictionary = ListDictionaryConverter.ToDictionary(DataManager.Instance.data.posData.positionData);

        foreach(string key in posDataDictionary.Keys)
        {
            GameObject target = GameObject.Find(key);
            if (target)
                target.transform.position = posDataDictionary[key];
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

        DataManager.Instance.data.posData.SetPositionDataFromDictionary(posDataDictionary);
    }
    void WriteMapData()
    {
        DataManager.Instance.data.worldData.SetWorldDataFromWorld(WorldManager.instance.world);
    }
    private void OnApplicationQuit()
    {
        WritePosData();
        WriteMapData();

        DataManager.Instance.SaveGameData();
    }

}
