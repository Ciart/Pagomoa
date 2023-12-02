using System;
using UnityEngine;

namespace Entities.Players
{
    /// <summary>
    /// Player를 Scene에서 제거하기 위한 임시 조치입니다.
    /// TODO: World에서 다른 Entity와 함께 처리하도록 변경해야합니다.
    /// </summary>
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public Entity playerEntity;

        public void Awake()
        {
            if (playerEntity.prefab.GetComponent<Entities.Players.PlayerController>() is null)
            {
                throw new Exception("PlayerController가 없는 Entity입니다.");
            }
        }

        public Entities.Players.PlayerController Spawn()
        {
            var entityManager = EntityManager.instance;

            return entityManager.SpawnPlayer(playerEntity, transform.position);
        }
    }
}