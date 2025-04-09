using System;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Mineral
    {
        public string id;
        
        public string name;
    
        public int tier;

        public string itemId;

        public Sprite? sprite;
        
        public TileBase tile;

        public Item item => ResourceSystem.instance.GetItem(itemId);
        
        private void LoadResources()
        {
            tile = Resources.Load<TileBase>($"Minerals/{id}");
            sprite = tile switch
            {
                Tile t => t.sprite,
                RuleTile t => t.m_DefaultSprite,
                RuleOverrideTile t => t.m_Sprites[0].m_OverrideSprite,
                _ => sprite
            };
        }

        public void Init()
        {
            LoadResources();
        }
    }
}
