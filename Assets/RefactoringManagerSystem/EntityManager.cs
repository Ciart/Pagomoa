using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities.Monsters;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using JetBrains.Annotations;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Entities
{
    public class EntityManager : Manager<EntityManager>
    {
        private List<EntityController> _entities = new();
        
        public override void PreStart()
        {
            Game.Instance.Time.RegisterTickEvent(SleepAndWakeUpMonsterEntity);   
        }
        public override void OnDestroy()
        {
            Game.Instance.Time.UnregisterTickEvent(SleepAndWakeUpMonsterEntity);
        }

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
            
            if (controller is PlayerController playerController)
            {
                EventManager.Notify(new PlayerSpawnedEvent(playerController));
            }
            
            return controller;
        }

        public void Despawn(EntityController controller)
        {
            if (!controller) return;
            _entities.Remove(controller);
            if (!controller.gameObject) return;
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

        public void SleepAndWakeUpMonsterEntity(int tick)
        {
            if (tick is not (TimeManager.Morning 
                or TimeManager.MaxTick - 4800)) return ;
            // 00시 부터 잠자기 시작
            
            _entities = _entities.Where(e => e != null).ToList();
            
            foreach (var entity in _entities)
            {
                if(!entity.CompareTag("Monster")) continue;
                entity.GetComponent<MonsterController>().StateChanged(tick != TimeManager.Morning
                    ? Monster.MonsterState.Sleep
                    : Monster.MonsterState.Active);
            }
        }
    }
}
