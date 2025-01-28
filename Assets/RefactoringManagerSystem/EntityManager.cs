using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : Manager<EntityManager>
    {
        private List<EntityController> _entities = new();

        [CanBeNull]
        public EntityController? Spawn(string id, Vector3 position, EntityStatus? status = null)
        {
            var entity = ResourceSystem.instance.GetEntity(id);

            if (entity.prefab == null)
            {
                Debug.LogError($"EntityManager: {id}의 프리팹이 존재하지 않습니다.");
                return null;
            }
            
            var controller = Object.Instantiate(entity.prefab, position, Quaternion.identity);
                       
            _entities.Add(controller);
            
            controller.Init(new EntityData(id, position.x, position.y, status));
            
            if (entity.tags.Contains("Player"))
            {
                var player = controller.GetComponent<PlayerController>();
                EventSystem.Notify(new PlayerSpawnedEvent(player));
            }
            
            return controller;
        }

        public void Despawn(EntityController controller)
        {
            _entities.Remove(controller);
            if (controller is null) return;
            Object.Destroy(controller.gameObject);
        }

        [CanBeNull]
        public EntityController Find(string id)
        {
            return _entities.Find(controller => controller.entityId == id);
        }

        public List<EntityController> FindAllEntityInChunk(Chunk chunk) 
        {
            // TODO: Quad-Tree로 최적화 해야 함.
            return _entities.FindAll((entity) => chunk.worldRect.Contains(entity.transform.position));
        }
    }
}
