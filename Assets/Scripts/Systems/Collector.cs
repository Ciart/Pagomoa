using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Systems
{
    public class Collector : MonoBehaviour
    {
        public UnityEvent OnCollectEvent;
        private void Awake()
        {
            if (OnCollectEvent == null)
                OnCollectEvent = new UnityEvent();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            var game = Game.Instance;
            
            if (!other.CompareTag("ItemEntityTrigger"))
            {
                return;
            }

            var itemEntity = other.transform.parent.GetComponent<ItemEntity>();
            var item = itemEntity.Item;

            if (item is null)
            {
                return;
            }

            var isMax = game.player.inventory.CheckMaxInventory(item.id);
            if (isMax) return;
            
            OnCollectEvent.Invoke();
            game.player.inventory.AddInventory(item.id);
            Destroy(itemEntity.gameObject);

            if (item.name == "PowerGemEarth")
            {
                game.hasPowerGemEarth = true;
            }
        }
    }
}
