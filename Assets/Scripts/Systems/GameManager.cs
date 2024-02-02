using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public bool isLoadSave;

        public bool hasPowerGemEarth;

        void Start()
        {
            var saveManager = SaveManager.Instance;
            bool mapLoad = saveManager.LoadMap();
            if (mapLoad)
            {
                saveManager.LoadPosition();
                saveManager.LoadItem();
                saveManager.LoadArtifactItem();
                saveManager.LoadQuickSlot();
                saveManager.LoadPlayerCurrentStatusData();
                saveManager.LoadEatenMineralCountData();
            }
            else
                saveManager.TagPosition(saveManager.loadPositionDelayTime);
        }

        private void Update()
        {
            if (hasPowerGemEarth)
            {
                Debug.Log("데모 종료");
            }
        }
    }
}
