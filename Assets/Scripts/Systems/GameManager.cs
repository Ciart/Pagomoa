using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public bool isLoadSave;

        public bool hasPowerGemEarth;

        private PlayerController _player;

        public static PlayerController player => instance._player;

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            _player = e.player;
        }

        void Start()
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

        private void Update()
        {
            if (hasPowerGemEarth)
            {
                Debug.Log("데모 종료");
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
    }
}