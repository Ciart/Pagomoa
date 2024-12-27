using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] public InventorySlotUI choiceSlot;
        [SerializeField] public InventorySlotUI hoverSlot;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private GameObject slot;
        [SerializeField] private Sprite emptyImage;
        public List<InventorySlotUI> slotData = new List<InventorySlotUI>();
        
        private const int MaxArtifactSlotData = 4;
        public InventorySlotUI[] artifactSlotData = new InventorySlotUI[MaxArtifactSlotData];

        private void Start()
        {
            MakeSlots();
            // QuickSlotContainerUI.instance.transform.SetAsLastSibling();
        }

        private void OnItemCountChanged(ItemCountChangedEvent e)
        {
            UpdateSlots();
        }

        private void OnEnable()
        {
            UpdateSlots();
            
            EventManager.AddListener<ItemCountChangedEvent>(OnItemCountChanged);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ItemCountChangedEvent>(OnItemCountChanged);
        }

        public void MakeSlots() // slotdatas 갯수만큼 슬롯 만들기
        {
            var gameManager = GameManager.instance;
            
            for (int i = 0; i < gameManager.player.inventory.items.Length; i++)
            {
                GameObject spawnedSlot = Instantiate(slot, slotParent.transform);
                slotData.Add(spawnedSlot.GetComponent<InventorySlotUI>());
                slotData[i].id = i;
                spawnedSlot.SetActive(true);
            }
            UpdateSlots();
        }
        
        public void UpdateSlots() // List안의 Item 전체 인벤토리에 출력
        {
            var gameManager = GameManager.instance;
            
            for (var i = 0; i < slotData.Count; i++)
            {
                slotData[i].SetItem(gameManager.player.inventory.items[i]);
            }
        }
        
        public void ResetSlots() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            foreach (var s in slotData)
                s.ResetItem();
        }
        
        public void SetArtifactSlots()
        {
            var gameManager = GameManager.instance;
            
            for (int i = 0; i < artifactSlotData.Length; i++)
            {
                artifactSlotData[i].slot = gameManager.player.inventory.artifactItems[i];
                artifactSlotData[i].SetItem(artifactSlotData[i].slot);
            }
        }
    }
}
