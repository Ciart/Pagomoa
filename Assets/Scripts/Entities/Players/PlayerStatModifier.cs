using System;

namespace Ciart.Pagomoa.Entities.Players
{
    [Serializable]
    public class PlayerStatusModifier
    {
        public float health = 0;
        
        public float healthMultiplier = 1;
        
        public float oxygen = 0;
        
        public float oxygenMultiplier = 1;
        
        public float hungry = 0;
        
        public float hungryMultiplier = 1;

        public float attack = 0;
        
        public float attackMultiplier = 1;
        
        public float defense = 0;
        
        public float defenseMultiplier = 1;
        
        public float sight = 0;
        
        public float sightMultiplier = 1;
        
        public float speed = 0;
        
        public float speedMultiplier = 1;
        
        public static PlayerStatusModifier operator +(PlayerStatusModifier a, PlayerStatusModifier b)
        {
            return new PlayerStatusModifier
            {
                health = a.health + b.health,
                healthMultiplier = a.healthMultiplier * b.healthMultiplier,
                oxygen = a.oxygen + b.oxygen,
                oxygenMultiplier = a.oxygenMultiplier * b.oxygenMultiplier,
                hungry = a.hungry + b.hungry,
                hungryMultiplier = a.hungryMultiplier * b.hungryMultiplier,
                attack = a.attack + b.attack,
                attackMultiplier = a.attackMultiplier * b.attackMultiplier,
                defense = a.defense + b.defense,
                defenseMultiplier = a.defenseMultiplier * b.defenseMultiplier,
                sight = a.sight + b.sight,
                sightMultiplier = a.sightMultiplier * b.sightMultiplier,
                speed = a.speed + b.speed,
                speedMultiplier = a.speedMultiplier * b.speedMultiplier
            };
        }
    }
}