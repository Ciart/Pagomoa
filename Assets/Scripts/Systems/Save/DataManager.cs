using System.IO;
using MemoryPack;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Save
{
    public class DataManager : MonoBehaviour
    {
        static private GameObject container;
        static private DataManager instance;
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
            var filePath = Application.persistentDataPath + "/" + GameDataFileName;
            if (File.Exists(filePath))
            {
                var FromJsonData = File.ReadAllText(filePath);
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
                OldSaveManager.instance.InitData();
                return false;
            }
        }

        public void DeleteGameData()
        {
            var filePath = Application.persistentDataPath + "/" + GameDataFileName;
            if(File.Exists(filePath))
                File.Delete(filePath);
            data = null;
        }

        public void SaveGameData()
        {
            var saveData = new SaveData()
            {
                worldSaveData = new WorldSaveData()
                {
                    
                }
            };
            
            var filePath = Application.persistentDataPath + "/" + GameDataFileName;
            var raw = MemoryPackSerializer.Serialize(saveData);
            File.WriteAllBytes(filePath, raw);
            Debug.Log("Data Saved");
        }
    }
}
