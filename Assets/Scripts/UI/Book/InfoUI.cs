﻿using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.UI.Book
{
    public class InfoUI : UIBehaviour
    {
        public TextMeshProUGUI[] texts;
        
        private void OnItemCountChangedEvent(ItemCountChangedEvent e)
        {
            var nextDrill = GameManager.player.drill.nextDrill;
            
            for (var i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var need = nextDrill.upgradeNeeds[i];

                if (e.item == need.mineral)
                {
                    texts[i].text = $"{need.mineral.name} ({e.count} / {need.count})";
                }
            }
        }

        private void Upgrade()
        {
            var nextDrill = GameManager.player.drill.nextDrill;
            
            for (var i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var need = nextDrill.upgradeNeeds[i];
                
                texts[i].text = $"{need.mineral.name} ({GameManager.player.inventory.GetItemCount(need.mineral)} / {need.count})";
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.AddListener<ItemCountChangedEvent>(OnItemCountChangedEvent);

            Upgrade();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.RemoveListener<ItemCountChangedEvent>(OnItemCountChangedEvent);
        }

        public void OnDrillUpgrade()
        {
            GameManager.player.drill.DrillUpgrade();
            Upgrade();
        }
    }
}