using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Move move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        move = GetComponent<Move>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (move.direction == 0 && Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("goLeft", false);
        } 
        else if (move.direction == 0 && Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetBool("goRight", false);
        }
        else if (move.direction == -1)
        {
            animator.SetBool("goRight", false);
            animator.SetBool("goLeft", true);
        }
        else if (move.direction == 1)
        {
            animator.SetBool("goLeft", false);
            animator.SetBool("goRight", true);
        }
    }
    void FixedUpdate()
    {
        
    }
}
