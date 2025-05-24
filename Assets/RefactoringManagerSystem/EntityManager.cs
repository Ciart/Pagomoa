using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities.Monsters;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : Manager<EntityManager>
    {
        private const string DEFAULT_LEVEL_ID = "__DEFAULT__";

        public override void PreStart()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            Game.Instance.Time.RegisterTickEvent(SleepAndWakeUpMonsterEntity);
        }
        public override void OnDestroy()
        {
            Game.Instance.Time.UnregisterTickEvent(SleepAndWakeUpMonsterEntity);
        }

        private Dictionary<string, List<EntityController>> _entities = new();

        public EntityController? Spawn(string id, Vector3 position, EntityStatus? status = null, string? levelId = null)
        {
            var entity = ResourceSystem.Instance.GetEntity(id);

            if (entity.prefab == null)
            {
                Debug.LogError($"EntityManager: {id}의 프리팹이 존재하지 않습니다.");
                return null;
            }

            var controller = Object.Instantiate(entity.prefab, position, Quaternion.identity);

            var actualLevelId = levelId ?? DEFAULT_LEVEL_ID;

            if (_entities.TryGetValue(actualLevelId, out var list))
            {
                list.Add(controller);
            }
            else
            {
                _entities[actualLevelId] = new List<EntityController> { controller };
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
            var entities = GetEntitiesInLevel(levelId).ToList();

            foreach (var entityController in entities)
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

        public List<EntityController> FindAllEntityInChunk(string levelId, Chunk chunk)
        {
            return GetEntitiesInLevel(levelId).FindAll(controller => chunk.worldRect.Contains(controller.transform.position));
        }

        public List<EntityController> GetEntitiesInLevel(string levelId)
        {
            if (_entities.TryGetValue(levelId, out var entities))
            {
                return entities;
            }

            return new List<EntityController>();
        }

        private void SleepAndWakeUpMonsterEntity(int tick)
        {
            if (tick is not (TimeManager.Morning
                or TimeManager.MaxTick - 6000)) return;

            foreach (var entityList in _entities)
            {
                foreach (var monster in entityList.Value)
                {
                    var controller = monster.GetComponent<MonsterController>();
                    if (!controller || monster.CompareTag("Monster")) continue;
                    controller.StateChanged(tick != TimeManager.Morning
                        ? Monster.MonsterState.Sleep
                        : Monster.MonsterState.Active);
                }
            }
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            foreach (var entityList in _entities)
            {
                entityList.Value.RemoveAll(entity => entity == null || entity.isDead);
            }
        }
    }
}
