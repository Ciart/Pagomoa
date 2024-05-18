using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    public class DungeonGate: MonoBehaviour
    {
        public string levelId;
        
        public void OnInteraction()
        {
            WorldManager.world.ChangeLevel(levelId);
        }
    }
}