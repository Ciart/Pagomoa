using System;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

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

        public float attack;
        
        public float defense;
        
        public float speed;

        /// <summary>
        /// Only Player
        /// </summary>
        public float oxygen;

        /// <summary>
        /// Only Player
        /// </summary>
        [FormerlySerializedAs("hungry")] public float hunger;

        public EntityController? prefab;

        public Sprite? portrait;
        
        private void LoadResources()
        {
            prefab = Resources.Load<EntityController>($"Entities/{id}");

            if (prefab != null)
            {
                portrait = prefab.GetComponent<EntityDialogue>()?.portrait;
            }
        }

        public void Init()
        {
            LoadResources();
        }
    }
}
