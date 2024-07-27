using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public Slot choiceSlot;
        [SerializeField] public Slot hoverSlot;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private GameObject slot;
        [SerializeField] private Sprite emptyImage;
        public List<Slot> slotDatas = new List<Slot>();
        
        private const int MaxArtifactSlotData = 4;
        public Slot[] artifactSlotData = new Slot[MaxArtifactSlotData];
        
        public static Inventory Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            MakeSlot();
            QuickSlotUI.instance.transform.SetAsLastSibling();
        }

        private void OnEnable()
        {
            DeleteSlot();
            ResetSlot();
        }
        public void MakeSlot() // slotdatas 갯수만큼 슬롯 만들기
        {
            for (int i = 0; i < GameManager.player.inventoryDB.items.Length; i++)
            {
                GameObject spawnedslot = Instantiate(slot, slotParent.transform);
                slotDatas.Add(spawnedslot.GetComponent<Slot>());
                slotDatas[i].id = i;
                spawnedslot.SetActive(true);
            }
            ResetSlot();
        }
        public void ResetSlot() // 각각 슬롯에 item 할당
        {
            int i = 0;
            for (; i < slotDatas.Count; i++)
            {
                slotDatas[i].inventoryItem =GameManager.player.inventoryDB.items[i];
            }
            UpdateSlot();
        }
        public void UpdateSlot() // List안의 Item 전체 인벤토리에 출력
        {
            for (int i = 0; i < GameManager.player.inventoryDB.items.Length; i++)
            {
                string convert = GameManager.player.inventoryDB.items[i].count.ToString();
                if (GameManager.player.inventoryDB.items[i].count == 0)
                    convert = "";
            
                if (GameManager.player.inventoryDB.items[i].item == null)
                    slotDatas[i].SetUI(emptyImage, convert);
                else
                    slotDatas[i].SetUI(GameManager.player.inventoryDB.items[i].item.itemImage, convert);
            }
        }
        public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            for (int i = 0; i < slotDatas.Count; i++)
                slotDatas[i].SetUI(emptyImage, "");
        }
        
        public void SetArtifactSlots()
        {
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                if (GameManager.player.inventoryDB.artifactItems[i].item != null)
                {
                    artifactSlotData[i].inventoryItem = GameManager.player.inventoryDB.artifactItems[i];
                    artifactSlotData[i].SetUI(artifactSlotData[i].inventoryItem.item.itemImage);
                }
                else
                {
                    artifactSlotData[i].SetUI(emptyImage);
                }
            }
        }
    }
}
