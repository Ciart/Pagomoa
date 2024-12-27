using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Wall
    {
        public string id;
        
        public string name;

        public bool isClimbable;
        
        public Sprite? sprite;

        public TileBase tile;
        
        private void LoadResources()
        {
            tile = Resources.Load<TileBase>($"Walls/{id}");
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
