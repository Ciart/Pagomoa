using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    [Serializable]
    public class Entity
    {
        public string id;

        public string[] tags = new string[0];
        
        public string name;

        public string description;

        public bool isEnemy;

        public bool isInvincible;
        
        public float baseHealth;

        public EntityController? prefab;
        
        private void LoadResources()
        {
            prefab = Resources.Load<EntityController>($"Entities/{id}");
        }

        public void Init()
        {
            LoadResources();
        }
    }
}
