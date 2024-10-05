using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : SingletonMonoBehaviour<EntityManager>
    {
        private static EntityManager _instance;

        private List<EntityController> _entities = new();

        public EntityController Spawn(EntityOrigin origin, Vector3 position, EntityStatus status = null)
        {
            var entity = Instantiate(origin.prefab, position, Quaternion.identity);
            _entities.Add(entity);

            entity.Init(new EntityData(position.x, position.y, origin, status));
            
            if (origin.type == EntityType.Player)
            {
                var player = entity.GetComponent<PlayerController>();
                EventManager.Notify(new PlayerSpawnedEvent(player));
            }

            return entity;
        }

        public void Despawn(EntityController controller)
        {
            _entities.Remove(controller);
            
            Destroy(controller.gameObject);
        }

        [CanBeNull]
        public EntityController Find(EntityOrigin origin)
        {
            return _entities.Find(controller => controller.origin == origin);
        }

        public List<EntityController> FindAllEntityInChunk(Chunk chunk) 
        {
            // TODO: Quad-Tree로 최적화 해야 함.
            return _entities.FindAll((entity) => chunk.worldRect.Contains(entity.transform.position));
        }
    }
}
