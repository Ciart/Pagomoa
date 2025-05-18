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
        private Dictionary<string?, List<EntityController>> _entities = new();

        public EntityController? Spawn(string id, Vector3 position, EntityStatus? status = null, string? levelId = null)
        {
            var entity = ResourceSystem.instance.GetEntity(id);

            if (entity.prefab == null)
            {
                Debug.LogError($"EntityManager: {id}의 프리팹이 존재하지 않습니다.");
                return null;
            }
            
            var controller = Object.Instantiate(entity.prefab, position, Quaternion.identity);

            if (_entities.TryGetValue(levelId, out var list))
            {
                list.Add(controller);
            }
            else
            {
                _entities[levelId] = new List<EntityController> { controller };
            }
            
            controller.Init(new EntityData(id, position.x, position.y, status));
            
            if (controller is PlayerController playerController)
            {
                EventManager.Notify(new PlayerSpawnedEvent(playerController));
            }
            
            return controller;
        }

        public void Despawn(EntityController controller)
        {
            if (!controller) return;
            
            foreach (var (_, list) in _entities)
            {
                if (list.Remove(controller))
                {
                    break;
                }
            }

            if (!controller.gameObject) return;
            Object.Destroy(controller.gameObject);
        }

        public void DespawnInLevel(string levelId)
        {
            foreach (var entityController in GetEntitiesInLevel(levelId))
            {
                Despawn(entityController);
            }
        }
        
        public EntityController? Find(string id)
        {
            foreach (var (_, list) in _entities)
            {
                var entity = list.Find(controller => controller.entityId == id);
                if (entity != null) return entity;
            }

            return null;
        }

        public List<EntityController> FindAllEntityInChunk(Chunk chunk) 
        {
            var result = new List<EntityController>();

            foreach (var (_, list) in _entities)
            {
                result.AddRange(list.FindAll(controller => chunk.worldRect.Contains(controller.transform.position)));
            }
            
            return result;
        }

        public List<EntityController> GetEntitiesInLevel(string levelId)
        {
            if (_entities.TryGetValue(levelId, out var entities))
            {
                return entities;
            }
            
            return new List<EntityController>();
        }
    }
}
