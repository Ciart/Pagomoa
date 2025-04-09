using System.IO;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Worlds;
using MemoryPack;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Save
{
    public class SaveSystem : SingletonMonoBehaviour<SaveSystem>
    {
        private const string GameDataFileName = "NewSave.dat";

        public SaveData? Data
        {
            get;
            private set;
        }

        public void Save()
        {
            Data = new SaveData();

            EventManager.Notify(new DataSaveEvent(Data));

            var path = Application.persistentDataPath + "/" + GameDataFileName;
            var raw = MemoryPackSerializer.Serialize(Data);
            File.WriteAllBytes(path, raw);
        }

        public void Load()
        {
            var path = Application.persistentDataPath + "/" + GameDataFileName;

            Data = MemoryPackSerializer.Deserialize<SaveData>(File.ReadAllBytes(path));

            if (Data == null)
            {
                return;
            }

            EventManager.Notify(new DataLoadedEvent(Data));
        }

        protected override void Awake()
        {
            base.Awake();
            
            if (PlayerPrefs.GetInt("SaveSlot") != 0)
            {
                Load();
            }
        }
    }
}
