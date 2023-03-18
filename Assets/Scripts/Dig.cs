using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour
{
    private PlayerController player;

    public bool Digging;
    [SerializeField] public Transform[] HitPointDown;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Digging = true;
    }
    void FixedUpdate()
    {
        if (Digging)
        {
            _Dig();
            Digging = false;
        }
    }
    public void _Dig()
    {
        bool CheckDig = false;
        for (int i = 0; i < HitPointDown.Length; i++)
        {
            Collider2D overCollider2d = Physics2D.OverlapCircle(HitPointDown[i].position, 0.1f, player.WhatisGround);
            if (overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Brick>().MakeDot(HitPointDown[i].position);
                CheckDig = true;
            }
        }
        if (CheckDig)
        {
            float hungrydeclineSpeed = 5;
            player.status.hungry -= hungrydeclineSpeed;
        }
        player.hungry_alter.Invoke();
    }
}
