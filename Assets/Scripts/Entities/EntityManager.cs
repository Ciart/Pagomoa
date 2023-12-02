using System;
using System.Collections.Generic;
using Entities.Players;
using JetBrains.Annotations;
using UnityEngine;
using Worlds;

namespace Entities
{
    public class EntityManager : MonoBehaviour
    {
        private PlayerController _player;
        public PlayerController player => _player;

        public event Action<PlayerController> spawnedPlayer;
        
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

        public EntityController Spawn(Entity entity, Vector3 position, EntityStatus status = null)
        {
            var controller = Instantiate(entity.prefab, position, Quaternion.identity);
            _entities.Add(controller);

            controller.Init(entity, status);

            return controller;
        }
        
        public PlayerController SpawnPlayer(Entity entity, Vector3 position)
        {
            if (_player is null)
            {
                _player = Spawn(entity, position).GetComponent<PlayerController>();
                spawnedPlayer?.Invoke(_player);
            }

            return _player;
        }

        public void Despawn(EntityController controller)
        {
            _entities.Remove(controller);
            
            Destroy(controller.gameObject);
        }

        [CanBeNull]
        public EntityController Find(Entity entity)
        {
            return _entities.Find(controller => controller.entity == entity);
        }

        public List<EntityController> FindAllEntityInChunk(Chunk chunk)
        {
            // TODO: Quad-Tree로 최적화 해야 함.
            return _entities.FindAll((entity) => chunk.worldRect.Contains(entity.transform.position));
        }
    }
}