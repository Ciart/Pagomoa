using System.IO;
using System.Threading.Tasks;
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

        // TODO: 입출력을 비동기 함수로 바꿔야 함.
        public async Awaitable Save()
        {
            Data = new SaveData();

            EventManager.Notify(new DataSaveEvent(Data));

            var path = Application.persistentDataPath + "/" + GameDataFileName;
            var raw = MemoryPackSerializer.Serialize(Data);
            File.WriteAllBytes(path, raw);
        }

        // TODO: 입출력을 비동기 함수로 바꿔야 함.
        public async Task Load(bool isFade = true)
        {
            var path = Application.persistentDataPath + "/" + GameDataFileName;

            Data = MemoryPackSerializer.Deserialize<SaveData>(File.ReadAllBytes(path));

            if (Data == null)
            {
                return;
            }

            if (!isFade)
            {
                EventManager.Notify(new DataLoadedEvent(Data));
                return;
            }

            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeIn, 1f);
            await Awaitable.WaitForSecondsAsync(1f);

            EventManager.Notify(new DataLoadedEvent(Data));

            await Game.Instance.World.WaitForLevelUpdate();

            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeOut, 1f);
            await Awaitable.WaitForSecondsAsync(1f);
        }

        // TODO: 세이브파일 없으면 이어하기 지우기
    }
}
