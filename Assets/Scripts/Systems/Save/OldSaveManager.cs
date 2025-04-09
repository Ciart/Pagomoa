using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI.Title;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa.Systems.Save
{
    public class OldSaveManager
    {
        static private GameObject container;

        public float loadPositionDelayTime = 1.5f;
        public bool LoadComplete = false;

        private WorldManager _worldManager;

        private WorldGenerator _worldGenerator;

        List<GameObject> ManagingTargets = new List<GameObject>();

        public void Awake()
        {
            if (SceneManager.GetActiveScene().name == "Title") return;

            //DataManager.Instance.LoadGameData();

            AddManagingTargetWithTag("Player");
            AddManagingTargetWithTag("Monster");
            AddManagingTargetWithTag("NPC");
        }
    
        private bool AllBlockNullCheck()
        {
            if (DataManager.Instance.data == null) return true;
            bool allNullCheck = true;
            // DicList<Vector2Int, Chunk> chunks = DataManager.Instance.data.worldData._chunks;
            // if (chunks == null) return true;
            // int dataSize = chunks.data.Count;
            // //Debug.Log("수량: " + dataSize);
            // for (int i = 0; i < dataSize; i++)
            // {
            //     int brickSize = chunks.data[i].Value.bricks.Length;
            //     for (int j = 0; j < brickSize; j++)
            //     {
            //         if (chunks.data[i].Value.bricks[j].ground != null)
            //             allNullCheck = false;
            //     }
            // }
            return allNullCheck;
        }

        private void AddManagingTargetWithTag(string tagName)
        {
            var targets = GameObject.FindGameObjectsWithTag(tagName);
            if (targets.Length == 0) return;
            foreach (var target in targets)
            {
                if (target.activeSelf)
                    ManagingTargets.Add(target);
            }
        }

        public void FreezePosition()
        {
            foreach (var target in ManagingTargets)
            {
                target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                target.GetComponent<Rigidbody2D>().Sleep();
            }
        }

        public void TagPosition(float time = 0)
        {
            
            // Invoke("_TagPosition", time);
        }

        private void _TagPosition()
        {
            LoadComplete = true;
            foreach (var target in ManagingTargets)
            {
                target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                target.GetComponent<Rigidbody2D>().WakeUp();
            }
        }

        public void LoadPosition()
        {
            // Invoke("_LoadPosition", loadPositionDelayTime);
        }

        private void _LoadPosition()
        {
            TagPosition();

            if (DataManager.Instance.data.posData == null)
                return;
            if (DataManager.Instance.data.posData.positionData == null)
                return;

            Dictionary<string, Vector3> posDataDictionary = ListDictionaryConverter.ToDictionary(DataManager.Instance.data.posData.positionData);
            if (posDataDictionary.Count != 0)
            {
                foreach (string key in posDataDictionary.Keys)
                {
                    GameObject target = GameObject.Find(key);
                    if (target)
                        target.transform.position = posDataDictionary[key];
                }
            }
        }

        public bool LoadMap()
        {
            var LoadSuccess = DataManager.Instance.LoadGameData();
            var game = Game.Instance;
            
            if (!game.isLoadSave || DataManager.Instance.data == null) LoadSuccess = false;
            if (DataManager.Instance.data != null)
            {
                if (DataManager.Instance.data.worldData == null) LoadSuccess = false;
                if (AllBlockNullCheck()) LoadSuccess = false;
            }

            //Debug.Log("블럭모두없음?: " + AllBlockNullCheck());

            FreezePosition();

            var worldManager = WorldManager.instance;

            if (game.isLoadSave && LoadSuccess)
            {
                try
                {
                    worldManager.worldGenerator.LoadWorld(DataManager.Instance.data.worldData);
                    return true;
                }
                catch
                {
                    worldManager.worldGenerator.Generate();
                    return false;
                }
            }
            else
            {
                worldManager.worldGenerator.Generate();
                return false;
            }
        }

        public void LoadItem()
        {
            /*var dataManager = DataManager.Instance;
            if (dataManager.data.itemData == null) return;

            if (dataManager.data.itemData.items != null)
            {
                var player = Game.instance.player;
                
                player.inventory.inventorySlots = dataManager.data.itemData.items;
                player.inventory.gold = dataManager.data.itemData.gold;
            }
            else
                Debug.Log("Item Data is Nothing");*/
        }
        
        // public void LoadArtifactItem()
        // {
        //     if(DataManager.Instance.data.artifactData != null && ArtifactSlotDB.Instance != null)
        //         if(DataManager.Instance.data.artifactData.artifacts != null)
        //             ArtifactSlotDB.Instance.Artifact = DataManager.Instance.data.artifactData.artifacts.ToList();
        // }   

        // public void LoadQuickSlot()
        // {
        //     var dataManager = DataManager.Instance;
        //     if (dataManager.data.quickSlotData == null) return;
        //
        //     if (dataManager.data.quickSlotData.items.Find(x => x.item) != null)
        //     {
        //         QuickSlotItemDB.instance.quickSlotItems = dataManager.data.quickSlotData.items/*.ToList()*/;
        //         QuickSlot.Instance.SetQuickSlot();
        //         QuickSlotItemDB.instance.ControlQuickSlot(dataManager.data.quickSlotData.selectedSlotID);
        //     }
        //     else
        //         Debug.Log("QuickSlot Data is Nothing");
        // }

        public void LoadPlayerCurrentStatusData()
        {
            /*var player = Game.instance.player;
            
            if (DataManager.Instance.data.playerStatusData == null) return;
            if (player == null) return;
            
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            playerStatus.Oxygen = DataManager.Instance.data.playerStatusData.currentOxygen;
            playerStatus.Hungry = DataManager.Instance.data.playerStatusData.currentHungry;
            playerStatus.oxygenAlter.Invoke(playerStatus.Oxygen, playerStatus.maxOxygen);
            playerStatus.hungryAlter.Invoke(playerStatus.Hungry, playerStatus.maxHungry);*/
        }

        public void LoadEatenMineralCountData()
        {
            if (DataManager.Instance.data.mineralData != null)
            {
                var player = Game.Instance.player;
                /*player.inventory.stoneCount = DataManager.Instance.data.mineralData.eatenMineralCount;*/
            }
                
            else
                Debug.Log("Mineral Data is Nothing");
        }

        private void WritePosData()
        {
            Dictionary<string, Vector3> posDataDictionary = new Dictionary<string, Vector3>();

            foreach (var target in ManagingTargets)
                posDataDictionary.Add(target.name, target.transform.position);

            InitData();

            DataManager.Instance.data.posData.SetPositionDataFromDictionary(posDataDictionary);
        }

        private void WriteMapData()
        {
            InitData();
            // DataManager.Instance.data.worldData.SetWorldDataFromWorld(WorldManager.instance.world);
        }

        public void WriteIntroData(bool arg)
        {
            InitData();
            DataManager.Instance.data.introData.isFirstStart = arg;
        }

        /*private void WriteItemData()
        {
            var player = Game.instance.player;
            
            InitData();
            DataManager.Instance.data.itemData.SetItemDataFromInventoryDB(player.inventory);
        }*/
       
        // private void WriteArtifactData()
        // {
        //     InitData();
        //     DataManager.Instance.data.artifactData.SetArtifactDataFromArtifactSlotDB(ArtifactSlotDB.Instance);
        // }

        // private void WriteQuickSlotData()
        // {
        //     InitData();
        //     DataManager.Instance.data.quickSlotData.SetQuickSlotDataFromQuickSlotDB(QuickSlotItemDB.instance);
        // }

        private void WritePlayerCurrentStatusData()
        {
            var player = Game.instance.player;
            
            InitData();
            DataManager.Instance.data.playerStatusData.SetCurrentStatusData(player.GetComponent<PlayerStatus>());
        }

        private void WriteEatenMineralCountData()
        {
            var player = Game.instance.player;
            
            InitData();
            /*DataManager.Instance.data.mineralData.SetEatenMineralData(player.inventory);*/
        }

        public void InitData()
        {
            if (DataManager.Instance.data == null)
                DataManager.Instance.data = new GameData();

            if (DataManager.Instance.data.posData == null)
            {
                Debug.Log("No Position Data before, Instantiate new Position Data");
                DataManager.Instance.data.posData = new PositionData();
            }
            if (DataManager.Instance.data.worldData == null)
            {
                Debug.Log("No World Data before, Instantiate new World Data");
                DataManager.Instance.data.worldData = new WorldData();
            }
            if (DataManager.Instance.data.introData == null)
            {
                Debug.Log("No Intro Data before, Instantiate new Intro Data");
                DataManager.Instance.data.introData = new IntroData();
            }
            if (DataManager.Instance.data.itemData == null)
            {
                Debug.Log("No Item Data before, Instantiate new Item Data");
                DataManager.Instance.data.itemData = new ItemData();
            }
            // if (DataManager.Instance.data.optionData == null)
            // {
            //     Debug.Log("No Option Data before, Instantiate new Option Data");
            //     DataManager.Instance.data.optionData = new OptionData();
            // }
            if (DataManager.Instance.data.artifactData == null)
            {
                Debug.Log("No Artifact Data before, Instantiate new Artifact Data");
                DataManager.Instance.data.artifactData = new ArtifactData();
            }
            if (DataManager.Instance.data.quickSlotData == null)
            {
                Debug.Log("No QuickSlot Data before, Instantiate new QuickSLot Data");
                DataManager.Instance.data.quickSlotData = new QuickSlotData();
            }
            if (DataManager.Instance.data.playerStatusData == null)
            {
                Debug.Log("No Player Status Data before, Instantiate new Player Status Data");
                DataManager.Instance.data.playerStatusData = new PlayerCurrentStatusData();
            }
            if (DataManager.Instance.data.mineralData == null)
            {
                Debug.Log("No Mineral Data before, Instantiate new Mineral Data");
                DataManager.Instance.data.mineralData = new MineralData();
            }
        }

        private void OnApplicationQuit()
        {
            if (!LoadComplete) return;
            WritePosData();
            WriteMapData();
            /*WriteItemData();*/
            // WriteArtifactData();
            // WriteQuickSlotData();
            WritePlayerCurrentStatusData();
            WriteEatenMineralCountData();
            DataManager.Instance.SaveGameData();
        }

    
    }
}
