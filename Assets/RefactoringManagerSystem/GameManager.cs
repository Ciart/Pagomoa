using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class GameManager : PManager<GameManager>
    {
        ~GameManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        public bool isLoadSave = true;

        public bool hasPowerGemEarth;

        public PlayerController player;
        
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
            var saveManager = SaveManager.Instance;
            bool mapLoad = saveManager.LoadMap();
            if (mapLoad)
            {
                saveManager.LoadPosition();
                saveManager.LoadItem();
                // saveManager.LoadArtifactItem();
                // saveManager.LoadQuickSlot();
                saveManager.LoadPlayerCurrentStatusData();
                saveManager.LoadEatenMineralCountData();
            }
            else
                saveManager.TagPosition(saveManager.loadPositionDelayTime);
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
