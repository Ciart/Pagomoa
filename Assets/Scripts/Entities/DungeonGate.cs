using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using System;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    public class DungeonGate: MonoBehaviour
    {
        public string levelId;

        public EntityOrigin destination;

        public Sprite[] images;

        public bool isEntable = false;

        public int breakCount = 3;

        public void OnInteraction()
        {
            if (!isEntable) return;

            WorldManager.world.ChangeLevel(levelId);

            var player = GameManager.player;
            var entityManager = EntityManager.instance;
            var destinationEntity = entityManager.Find(destination);
            
            if (destinationEntity is null)
            {
                player.transform.position = Vector3.zero;
                return;
            }
            
            player.transform.position = destinationEntity.transform.position;
        }

        private void OnEnable()
        {
            var entityController = GetComponent<EntityController>();
            entityController.exploded += BreakDoor;
            GetComponent<SpriteRenderer>().sprite = images[0];
            Debug.Log("문 이벤트 등록 완료");
        }

        private void OnDisable()
        {
            GetComponent<EntityController>().exploded -= BreakDoor;
        }

        public void BreakDoor(EntityExplodedEventArgs args)
        {
            breakCount -= 1;
            if(breakCount == 3)
                GetComponent<SpriteRenderer>().sprite = images[1];
            if (breakCount == 2)
                GetComponent<SpriteRenderer>().sprite = images[2];
            if (breakCount < 1)
            {
                GetComponent<SpriteRenderer>().sprite = images[3];
                isEntable = true;
                GetComponent<EntityController>().exploded -= BreakDoor;
            }
        }
    }
}