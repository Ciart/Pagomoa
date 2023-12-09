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

    public bool LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            try
            {
                data = JsonUtility.FromJson<GameData>(FromJsonData);
                Debug.Log("Data Loaded From : " + filePath);
            }
            catch (System.Exception e)
            {
                Debug.Log("bug : " + e);
            }
            return true;
        }
        else
        {
            SaveManager.Instance.InitData();
            return false;
        }
    }
    public void DeleteGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if(File.Exists(filePath))
            File.Delete(filePath);
        data = null;
    }
    public void SaveGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        string ToJasonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, ToJasonData);
        Debug.Log("Data Saved");
    }
}
