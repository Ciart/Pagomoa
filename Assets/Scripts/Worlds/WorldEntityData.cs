using System;
using Ciart.Pagomoa.Entities;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class WorldEntityData
    {
        public float x;

        public float y;

        public Entity entity;

        public EntityStatus status;

        public WorldEntityData(float x, float y, Entity entity, EntityStatus status = null)
        {
            this.x = x;
            this.y = y;
            this.entity = entity;
            this.status = status;
        }
    }
}
