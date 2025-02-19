using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellUI : MonoBehaviour
    {
        [SerializeField] private SellSlotUI instanceSlotUI;
        private List<SellSlotUI> _sellSlots = new List<SellSlotUI>();

        private void Awake()
        {
            MakeSellUISlot(); 
        }

        private void OnEnable()
        {
            ClearSellUISlot();
            UpdateSellUISlot();
        }
        
        public void MakeSellUISlot()
        {
            for(int i = 0; i < Inventory.MaxInventorySlots; i++)
            {
                var spawnedSlot = Instantiate(instanceSlotUI, transform);
                spawnedSlot.SetSlotID(i);
                spawnedSlot.AddSlotEvent();
                spawnedSlot.gameObject.SetActive(true);
                _sellSlots.Add(spawnedSlot);
            }
            UpdateSellUISlot();
        }
        public void UpdateSellUISlot()
        {
            var inventory = Game.Instance.player.inventory;
            var inventorySlots = inventory.GetSlots(SlotType.Inventory);
            if (inventorySlots == null) return;
            
            for (int i = 0; i < _sellSlots.Count; i++)
            {
                _sellSlots[i].SetSlot(inventorySlots[i]);
            }
        }
        public void ClearSellUISlot()
        {
            var emptySlot = new Slot();
            emptySlot.SetSlotItemID("");
            foreach (var slot in _sellSlots)
                slot.SetSlot(emptySlot);
        }
    }
}
