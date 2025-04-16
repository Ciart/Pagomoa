using System;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Systems
{
    public class Collector : MonoBehaviour
    {
        private bool TryAddItemInInventory(string itemId)
        {
            var inventory = Game.Instance.player?.inventory;

            if (inventory == null) return false;
            
            inventory.AddInventory(itemId);

            if (itemId == "PowerGemEarth")
            {
                Game.Instance.hasPowerGemEarth = true;
            }

            return true;
        }

        private void CollectItem(Collider2D other)
        {
            if (!other.CompareTag("ItemEntityTrigger"))
            {
                return;
            }

            var itemEntity = other.transform.parent.GetComponent<ItemEntity>();
            var item = itemEntity.Item;

            if (TryAddItemInInventory(item.id))
            {
                Destroy(itemEntity.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CollectItem(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            CollectItem(other);
        }
    }
}
