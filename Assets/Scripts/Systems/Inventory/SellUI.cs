using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class SellUI : MonoBehaviour
    {
        [SerializeField] private SellSlot _instanceSlot;
        private List<SellSlot> _sellSlots = new List<SellSlot>();

        private void Awake()
        {
            MakeSellUISlot(); 
        }

        private void OnEnable()
        {
            DeleteSellUISlot();
            UpdateSellUISlot();
        }
        
        public void MakeSellUISlot()
        {
            var inventory = GameManager.instance.player.inventory;
            
            for(int i = 0; i < inventory.inventorySlots.Length; i++)
            {
                var spawnedSlot = Instantiate(_instanceSlot, transform);
                _sellSlots.Add(spawnedSlot);
                _sellSlots[i].id = i;
                spawnedSlot.gameObject.SetActive(true);
            }
            UpdateSellUISlot();
        }
        public void UpdateSellUISlot()
        {
            var inventory = GameManager.instance.player.inventory;

            for (int i = 0; i < _sellSlots.Count; i++)
            {
                _sellSlots[i].SetSlot(inventory.inventorySlots[i]);
            }
        }
        public void DeleteSellUISlot()
        {
            foreach (var slot in _sellSlots)
                slot.ResetSlot();
        }
    }
}