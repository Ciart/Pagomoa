using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.RefactoringManagerSystem;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : PManager<EntityManager>
    {
        private static EntityManager _instance;

        private List<EntityController> _entities = new();

        public EntityController Spawn(string id, Vector3 position, EntityStatus status = null)
        {
            ResourceSystem.instance.entities.TryGetValue(id, out var entity);

            if (entity == null)
            {
                Debug.LogError($"EntityManager: {id}는 존재하지 않습니다.");
            }
            
            var controller = Object.Instantiate(entity.prefab, position, Quaternion.identity);
            
            _entities.Add(controller);
            
            controller.Init(new EntityData(id, position.x, position.y, status));
            
            if (entity.tags == "Player")
            {
                var player = controller.GetComponent<PlayerController>();
                EventManager.Notify(new PlayerSpawnedEvent(player));
            }
            
            return controller;
        }

        public void Despawn(EntityController controller)
        {
            _entities.Remove(controller);
            
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
