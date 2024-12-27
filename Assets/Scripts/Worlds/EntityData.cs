using System;
using Ciart.Pagomoa.Entities;
using JetBrains.Annotations;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Worlds
{
    [Serializable]
    public class EntityData
    {
        public string id;

        public float x;

        public float y;

        [CanBeNull] public EntityStatus status;

        public EntityData(string id, float x, float y, EntityStatus status = null)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.status = status;
        }
    }
}
