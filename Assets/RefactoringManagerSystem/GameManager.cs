using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class GameManager : PManager<GameManager>
    {
        public bool isLoadSave = true;

        public bool hasPowerGemEarth;

        public PlayerController? player;

        ~GameManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            player = e.player;
        }

        public override void Awake()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        public override void Start()
        {
            // var saveManager = OldSaveManager.instance;
            // bool mapLoad = saveManager.LoadMap();
            // if (mapLoad)
            // {
            //     saveManager.LoadPosition();
            //     saveManager.LoadItem();
            //     // saveManager.LoadArtifactItem();
            //     // saveManager.LoadQuickSlot();
            //     saveManager.LoadPlayerCurrentStatusData();
            //     saveManager.LoadEatenMineralCountData();
            // }
            // else
            //     saveManager.TagPosition(saveManager.loadPositionDelayTime);
        }

        public override void Update()
        {
            if (hasPowerGemEarth)
            {
                Debug.Log("데모 종료");
            }
        }
    }
}
