using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] public InventorySlotUI choiceSlot;
        [SerializeField] public InventorySlotUI hoverSlot;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private GameObject slot;
        [SerializeField] private Sprite emptyImage;
        public List<InventorySlotUI> slotDatas = new List<InventorySlotUI>();
        
        private const int MaxArtifactSlotData = 4;
        public InventorySlotUI[] artifactSlotData = new InventorySlotUI[MaxArtifactSlotData];
        
        public static InventoryUI Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            MakeSlots();
            // QuickSlotContainerUI.instance.transform.SetAsLastSibling();
        }

        private void OnEnable()
        {
            UpdateSlots();
        }
        
        public void MakeSlots() // slotdatas 갯수만큼 슬롯 만들기
        {
            for (int i = 0; i < GameManager.player.inventory.items.Length; i++)
            {
                GameObject spawnedslot = Instantiate(slot, slotParent.transform);
                slotDatas.Add(spawnedslot.GetComponent<InventorySlotUI>());
                slotDatas[i].id = i;
                spawnedslot.SetActive(true);
            }
            UpdateSlots();
        }

        public void UpdateSlots() // List안의 Item 전체 인벤토리에 출력
        {
            for (var i = 0; i < slotDatas.Count; i++)
            {
                slotDatas[i].SetItem(GameManager.player.inventory.items[i]);
            }
        }
        
        public void ResetSlots() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            foreach (var s in slotDatas)
                s.ResetItem();
        }
        
        public void SetArtifactSlots()
        {
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                artifactSlotData[i].slot = GameManager.player.inventory.artifactItems[i];
                artifactSlotData[i].SetItem(artifactSlotData[i].slot);
            }
        }
    }
}
