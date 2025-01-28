using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine.EventSystems;
using EventSystem = Ciart.Pagomoa.Events.EventSystem;

namespace Ciart.Pagomoa.UI.Book
{
    public class InfoUI : UIBehaviour
    {
        public TextMeshProUGUI[] texts;
        
        private void OnItemCountChangedEvent(ItemCountChangedEvent e)
        {
            var nextDrill = Game.instance.player.drill.nextDrill;
            
            for (var i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var need = nextDrill.upgradeNeeds[i];

                if (e.itemID == need.mineral.id)
                {
                    texts[i].text = $"{need.mineral.name} ({e.count} / {need.count})";
                }
            }
        }

        private void Upgrade()
        {
            var nextDrill = Game.instance.player.drill.nextDrill;
            
            for (var i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var need = nextDrill.upgradeNeeds[i];
                
                texts[i].text = $"{need.mineral.name} ({Game.instance.player.inventory.GetItemCount(need.mineral)} / {need.count})";
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            EventSystem.AddListener<ItemCountChangedEvent>(OnItemCountChangedEvent);

            Upgrade();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventSystem.RemoveListener<ItemCountChangedEvent>(OnItemCountChangedEvent);
        }

        public void OnDrillUpgrade()
        {
            Game.instance.player.drill.DrillUpgrade();
            Upgrade();
        }
    }
}
