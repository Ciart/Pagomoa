using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ItemPlantBombEffect", menuName = "New ItemEffect/itemPlantBombEffect")]
[Serializable]
public class PlantBombEffect : ItemEffect
{
    [SerializeField] private GameObject prefab;

    public override void Effect(ConsumerableItem item, Status stat)
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null) return;
        GameObject Bomb = Instantiate(prefab);
        Bomb.transform.position = playerTransform.position + new Vector3(0, -1);
    }
}
