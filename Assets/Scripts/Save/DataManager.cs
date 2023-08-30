using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "Data Manager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    string GameDataFileName = "GameData.json";

    public GameData data = new GameData();

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        Debug.Log("load ${Application.persistentDataPath} filedata");
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            try
            {
                data = JsonUtility.FromJson<GameData>(FromJsonData);
            }
            catch (System.Exception e)
            {
                Debug.Log("bug : " + e);
            }
        }
        else
            SaveManager.Instance.InitData();
    }
    public void DeleteGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if(File.Exists(filePath))
            File.Delete(filePath);
        data.worldData = null;
        data.itemData = null;
        data.posData = null;
    }
    public void SaveGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        string ToJasonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, ToJasonData);
        Debug.Log("saved");
    }
}
