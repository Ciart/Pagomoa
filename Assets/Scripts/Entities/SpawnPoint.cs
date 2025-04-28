using Ciart.Pagomoa.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Entities
{
    /// <summary>
    /// Player를 Scene에서 제거하기 위한 임시 조치입니다.
    /// TODO: World에서 다른 Entity와 함께 처리하도록 변경해야합니다.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        public string entityId;

        private void Start()
        {
            Game.Instance.Entity.Spawn(entityId, transform.position);
        }

        /*private void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"SpawnPoint: {entityOrigin.displayName ?? "Null"}");
        }*/
    }
}