using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New ItemPlantBombEffect", menuName = "New ItemEffect/itemPlantBombEffect")]
    [Serializable]
    public class PlantBombEffect : ItemEffect
    {
        [SerializeField] private GameObject prefab;

        public override void Effect(ConsumableItem item, PlayerStatus stat)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform == null) return;
            GameObject Bomb = Instantiate(prefab);
            Bomb.transform.position = playerTransform.position + new Vector3(0, -1);
        }
    }
}
