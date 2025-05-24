using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities
{
    public class DesertPotController : MonoBehaviour
    {
        public string spawnEntityId = "";

        private EntityController _entityController = null!;

        private void Awake()
        {
            _entityController = GetComponent<EntityController>();
        }

        private void OnEnable()
        {
            _entityController.died += OnDied;
        }

        private void OnDisable()
        {
            _entityController.died -= OnDied;
        }

        private void OnDied(EntityDiedEventArgs e)
        {
            var currentLevel = WorldManager.world.currentLevel;
            Game.Instance.Entity.Spawn(spawnEntityId, transform.position, levelId: currentLevel.id);
        }
    }

}
