using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerDirection : MonoBehaviour
{
    public enum Facing
    {
        Up,
        Right,
        Down,
        Left
    }
    public float walkSpeed = 1;
    public bool blockDiagonals = false;
    public int playerID = 0;

    public Facing facingDir;
    public Vector3 facingVector;

    Animator anim;
    Vector2 movementVelocity;
    Rigidbody2D rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // lock diagonals
        if (blockDiagonals)
        {
            if (Mathf.Abs(v) >= Mathf.Abs(h) && v != 0)
            {
                h = 0;
            }
        }

        // if we are moving
        if (Mathf.Abs(h) + Mathf.Abs(v) != 0)
        {

            // check facing direction
            if (Mathf.Abs(v) >= Mathf.Abs(h))
            {
                if (v > 0)
                {
                    facingDir = Facing.Up;
                    facingVector = -Vector2.right;
                }
                else
                {
                    facingDir = Facing.Down;
                    facingVector = Vector2.right;
                }
            }
            else
            {
                if (h > 0)
                {
                    facingDir = Facing.Right;
                    facingVector = Vector2.up;

                }
                else
                {
                    facingDir = Facing.Left;
                    facingVector = -Vector2.up;
                }
            }
        }

        movementVelocity = new Vector2(h, v) * walkSpeed;
    }
}
