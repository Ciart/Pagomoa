using System;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "New Entity", menuName = "Pagomoa/Entity")]
    public class Entity : ScriptableObject
    {
        public string displayName;

        public float health = float.PositiveInfinity;
        
        public bool isEnemy;
        
        public string description;
    }
}