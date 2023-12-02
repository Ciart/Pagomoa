using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Players;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ItemPlantBombEffect", menuName = "New ItemEffect/itemPlantBombEffect")]
[Serializable]
public class PlantBombEffect : ItemEffect
{
    [SerializeField] private GameObject prefab;

    public override void Effect(ConsumerableItem item, PlayerStatus stat)
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null) return;
        GameObject Bomb = Instantiate(prefab);
        Bomb.transform.position = playerTransform.position + new Vector3(0, -1);
    }
}
