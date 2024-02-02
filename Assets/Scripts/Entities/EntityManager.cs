using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : MonoBehaviour
    {
        private PlayerController _player;
        public PlayerController player => _player;

        /// <summary>
        /// Player가 생성될 때 호출됩니다.
        /// Awake에서 구독하기를 권장합니다.
        /// </summary>
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
            
            if (entity.type == EntityType.Player)
            {
                _player = controller.GetComponent<PlayerController>();
                spawnedPlayer?.Invoke(_player);
            }

            return controller;
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