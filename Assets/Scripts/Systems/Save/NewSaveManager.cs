using System.IO;
using MemoryPack;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Save
{
    public class NewSaveManager : PManager<NewSaveManager>
    {
        private const string GameDataFileName = "NewSave.dat";
        
        public void Save()
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

        public void Load()
        {
            
        }

        public override void Awake()
        {
            Load();
        }

        public override void Quit()
        {
            Save();
        }
    }
}