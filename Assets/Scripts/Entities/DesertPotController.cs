using Ciart.Pagomoa.Entities;
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
            EntityManager.instance.Spawn(spawnEntityId, transform.position);
        }
    }

}
