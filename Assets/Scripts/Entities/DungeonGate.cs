using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    public class DungeonGate: MonoBehaviour
    {
        public string levelId;

        public EntityOrigin destination;
        
        public void OnInteraction()
        {
            WorldManager.world.ChangeLevel(levelId);

            var entityManager = EntityManager.instance;
            var player = entityManager.player;
            var destinationEntity = entityManager.Find(destination);
            
            if (destinationEntity is null)
            {
                player.transform.position = Vector3.zero;
                return;
            }
            
            player.transform.position = destinationEntity.transform.position;
        }
    }
}