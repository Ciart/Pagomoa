using System;
using UnityEngine;

namespace Entities
{
    public enum EntityType
    {
        Object,
        Player,
        Enemy,
        NPC,
    }
    
    public class Entity : MonoBehaviour
    {
        public 

        public string name;
        
        public float health = 10f;
    }
}