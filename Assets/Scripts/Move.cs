using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]

public class Move : MonoBehaviour
{
    private PlayerController player;
    
    private Rigidbody2D m_rigidbody;

    private Animator _animator;

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
        _animator = GetComponent<Animator>();
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

        PlayerClimbingAnimation();

        m_rigidbody.velocity = Vector2.SmoothDamp(m_rigidbody.velocity, TargetVelocity, ref v, 0.06f);
        player.SetDirection(direction);

        if (jump && isGround)
            m_rigidbody.AddForce(new Vector2(0, jumpForce));

        jump = false;
        crawlUp = false;

        StartCoroutine(EndMotion()) ;
    }

    void PlayerClimbingAnimation()
    {
        RaycastHit2D leftTileHit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 0.42f, LayerMask.GetMask("Platform"));
        RaycastHit2D rightTileHit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 0.42f, LayerMask.GetMask("Platform"));

        if (leftTileHit.collider && crawlUp && player.GroundHeight >= transform.position.y)
        {
            player.ActiveLeftTileClimbingAnimation();
            player.ActiveClimbingAnimation();
        }
        else if (rightTileHit.collider && crawlUp && player.GroundHeight >= transform.position.y)
        {
            player.ActiveRightTileClimbingAnimation();
            player.ActiveClimbingAnimation();
        }
        else if (crawlUp && player.GroundHeight >= transform.position.y)
        {
            player.DisableClimbingAnimation();
            player.DisableTileClimbingAnimation();
            player.ActiveClimbingAnimation();
        }
        else if (!crawlUp || player.GroundHeight < transform.position.y)
        {
            player.DisableClimbingAnimation();
            player.DisableTileClimbingAnimation();
        }
    }
    IEnumerator EndMotion()
    {
        yield return new WaitForSeconds(1.0f);
        if (transform.position.y >= player.GroundHeight && transform.position.y < 1.0f)
        {
            player.transform.position = new Vector3(Mathf.Floor(player.transform.position.x), 2.0f, player.transform.position.z);
            Debug.Log("x : " + Mathf.Floor(player.transform.position.x));
            Debug.Log("y : " + Mathf.Floor(player.transform.position.y));
            Debug.Log("z : " + Mathf.Floor(player.transform.position.z));
            player.ActiveLeftTileClimbingEndMotion();
        }
    }
}
