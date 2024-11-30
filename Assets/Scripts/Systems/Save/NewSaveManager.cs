using System.IO;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Worlds;
using MemoryPack;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Save
{
    public class NewSaveManager : PManager<NewSaveManager>
    {
        private const string GameDataFileName = "NewSave.dat";
        
        public void Save()
        {
            var data = new SaveData();
            
            EventManager.Notify(new DataSaveEvent(data));
            
            var path = Application.persistentDataPath + "/" + GameDataFileName;
            var raw = MemoryPackSerializer.Serialize(data);
            File.WriteAllBytes(path, raw);
        }

        public void Load()
        {
            var path = Application.persistentDataPath + "/" + GameDataFileName;
            var data = MemoryPackSerializer.Deserialize<SaveData>(File.ReadAllBytes(path));
            
            EventManager.Notify(new DataLoadedEvent(data));
        }

        // public override void Awake()
        // {
        //     Load();
        // }

        public override void Quit()
        {
            Save();
        }
    }
}
