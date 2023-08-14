using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/useitem/Bomb")]
[Serializable]
public class Bomb : Item
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private GameObject prefab;
    public override void Active(Status status = null)
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        x = player.position.x;
        y = player.position.y - 1;
        GameObject Bomb = Instantiate(prefab);
        Bomb.transform.position = new Vector3(x, y);
    }
}
