using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Entities
{
    [Serializable]
    public class Entity
    {
        public string id;

        public string tags;
        
        public string name;

        public string description;

        public bool isEnemy;

        public bool isInvincible;
        
        public float baseHealth;

        public EntityController prefab;
        
        public void LoadResources()
        {
            prefab = Resources.Load<EntityController>($"Entities/{id}");
        }
    }
}
