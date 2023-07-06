using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float hp = 0;
    public float moveSpeed = 0;
    public float damage = 0;
    public int dayNight = 1; // 1 : 낮, 0 : 밤
    
    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.transform.name == "Player")
        {
            collision2D.transform.GetComponent<PlayerController>().Hit(damage, transform.position);
        }
    }
}
