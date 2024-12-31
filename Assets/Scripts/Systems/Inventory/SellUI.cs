using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellUI : MonoBehaviour
    {
        [SerializeField] private SellSlot _instanceSlot;
        private List<InventorySlot> _sellSlots = new List<InventorySlot>();

        private void Awake()
        {
            MakeSellUISlot(); 
        }

        private void OnEnable()
        {
            DeleteSellUISlot();
            ResetSellUISlot();
        }
        
        public void MakeSellUISlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for(int i = 0; i < inventory.items.Length; i++)
            {
                var spawnedSlot = Instantiate(_instanceSlot, transform);
                _sellSlots.Add(spawnedSlot.GetComponent<InventorySlot>());
                _sellSlots[i].id = i;
                spawnedSlot.gameObject.SetActive(true);
            }
            ResetSellUISlot();
        }
        public void ResetSellUISlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for(int i = 0; i < _sellSlots.Count; i++)
                _sellSlots[i] = inventory.items[i];
            UpdateSellUISlot();
        }
        public void UpdateSellUISlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for (int i = 0; i < inventory.items.Length; i++)
            {
                _sellSlots[i].SetSlot(inventory.items[i].GetSlotItem());
            }
        }
        public void DeleteSellUISlot()
        {
            if (GameManager.instance.player.inventory.items.Length >= 0)
            {
                for (int i = 0; i < _sellSlots.Count; i++)
                    _sellSlots[i].ResetSlot();
            }
        }
    }
}