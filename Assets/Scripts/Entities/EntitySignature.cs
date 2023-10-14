using System;
using UnityEngine;

namespace Entities
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

    [CreateAssetMenu(fileName = "New EntitySignature", menuName = "Pagomoa/EntitySignature")]
    public class EntitySignature : ScriptableObject
    {
        public string displayName;

        public string description;

        public EntityType type;

        public int subType;
    }
}