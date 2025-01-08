using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class SnakeController : MonoBehaviour
    {
        private EntityController _entityController;

        private void Awake() {
            _entityController = GetComponent<EntityController>();
        }
    }
}
