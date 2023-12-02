using System;
using System.Collections.Generic;
using UnityEngine;
using Worlds;

namespace Entities
{
    public class EntityManager : MonoBehaviour
    {
        private static EntityManager _instance;

        private List<EntityController> _entities = new();

        public static EntityManager instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (EntityManager)FindObjectOfType(typeof(EntityManager));
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public EntityController SpawnEntity(Entity entity, Vector3 position, EntityStatus status = null)
        {
            var controller = Instantiate(entity.prefab, position, Quaternion.identity);
            _entities.Add(controller);

            controller.Init(entity, status);

            return controller;
        }

        public void DespawnEntity(EntityController controller)
        {
            _entities.Remove(controller);
            
            Destroy(controller.gameObject);
        }

        public List<EntityController> FindAllEntityInChunk(Chunk chunk)
        {
            // TODO: Quad-Tree로 최적화 해야 함.
            return _entities.FindAll((entity) => chunk.worldRect.Contains(entity.transform.position));
        }
    }
}