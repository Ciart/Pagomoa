﻿using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    public enum EntityType
    {
        Null,
        Player,
        Npc,
        Item,

        // Enemy
        Snake = 100,
        Mummy,
        Clover,
        Owl,
        Golem
    }

    [CreateAssetMenu(fileName = "New Entity Definition", menuName = "Pagomoa/Entity")]
    public class EntityOrigin : ScriptableObject
    {
        public string displayName;

        public string description;

        public EntityType type;

        public int subType;

        public bool isEnemy;

        public bool isInvincible;
        
        public EntityController prefab;
        
        public float baseHealth;
    }
}
