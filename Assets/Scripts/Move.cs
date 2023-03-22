using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]

public class Move : MonoBehaviour
{
    private PlayerController player;

    private Rigidbody2D m_rigidbody;

    public float direction;
    private bool jump;
    private bool crawlUp;
    private bool isGround;

    public Transform groundCheck;
        
    public float speed = 5f;
    public float jumpForce = 465f;
    public float crawlSpeed = 5f;
    // Start is called before the first frame update

    void Awake()
    {
        player = GetComponent<PlayerController>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
            isGround = false;
        }
        if (Input.GetKey(KeyCode.C))
            crawlUp = true;

        direction = Input.GetAxisRaw("Horizontal");
    }
    void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, 0.2f, player.WhatisGround);
        if(collider != null)
            isGround = true;


        Vector2 TargetVelocity;
        TargetVelocity = crawlUp && player.GroundHeight >= transform.position.y ? new Vector2(direction * speed, crawlSpeed) : new Vector2(direction * speed, m_rigidbody.velocity.y);
        Vector2 v = Vector2.zero;

        m_rigidbody.velocity = Vector2.SmoothDamp(m_rigidbody.velocity, TargetVelocity, ref v, 0.06f);
        player.SetDirection(direction);

        if (jump && isGround)
            m_rigidbody.AddForce(new Vector2(0, jumpForce));

        jump = false;
        crawlUp = false;
    }
}
