using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class Ground
    {
        public string id;
        
        public string name;
        
        public float strength;
        
        public string color;
        
        public Sprite? sprite;

        public TileBase tile;
        
        private void LoadResources()
        {
            tile = Resources.Load<TileBase>($"Grounds/{id}");
            sprite = tile switch
            {
                Tile t => t.sprite,
                RuleTile t => t.m_DefaultSprite,
                RuleOverrideTile t => t.m_Sprites[0].m_OverrideSprite,
                _ => null
            };
        }

        public void Init()
        {
            LoadResources();
        }
    }
}
