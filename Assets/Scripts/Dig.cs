using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Dig : MonoBehaviour
{
    private PlayerController player;
    public float direction;
    public bool DiggingDown;
    public bool DiggingHorizontal;
   
    [SerializeField] public Transform HitPointDown;
    [SerializeField] public Transform HitPointHorizontal;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(direction == 0)
                DiggingDown = true;
            
            else if(direction != 0)
                DiggingHorizontal = true;
        }
    }
    void FixedUpdate()
    {
        if (DiggingDown)
        {
            _Dig();
            DiggingDown = false;
        }
        else if (DiggingHorizontal)
        {
            _Dig();
            DiggingHorizontal = false;
        }
        
    }
    public void _Dig()
    {
       
        bool CheckDig = false;
        if (DiggingDown)
        {
            Collider2D overCollider2d = Physics2D.OverlapCircle(HitPointDown.position, 0.1f, player.WhatisGround);
            Vector3 SizeL = new Vector3(HitPointDown.position.x - 0.3f, HitPointDown.position.y, HitPointDown.position.z);
            Vector3 SizeR = new Vector3(HitPointDown.position.x + 0.3f, HitPointDown.position.y, HitPointDown.position.z);
            if (overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Brick>().MakeDot(SizeL);
                overCollider2d.transform.GetComponent<Brick>().MakeDot(SizeR);
                CheckDig = true;
            }
        }
        else if (DiggingHorizontal)
        {
            
            Collider2D overCollider2d = Physics2D.OverlapCircle(HitPointHorizontal.position, 0.1f, player.WhatisGround);
            
            Vector3 SizeU = new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y + 0.3f, HitPointHorizontal.position.z);
            Vector3 SizeD = new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y - 0.3f, HitPointHorizontal.position.z);
            if (overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Brick>().MakeDot(SizeU);
                overCollider2d.transform.GetComponent<Brick>().MakeDot(SizeD);
                CheckDig = true;
            }
        }

        if (CheckDig)
        {
            float hungrydeclineSpeed = 5;
            player.status.hungry -= hungrydeclineSpeed;
            player.hungry_alter.Invoke();
        }
    }
}
