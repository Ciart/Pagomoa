using UnityEngine;

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
        Owl
    }

    [CreateAssetMenu(fileName = "New Entity", menuName = "Pagomoa/Entity")]
    public class Entity : ScriptableObject
    {
        public string displayName;

        public string description;

        public EntityType type;

        public int subType;
        
        public EntityController prefab;
        
        public float baseHealth;
    }
}