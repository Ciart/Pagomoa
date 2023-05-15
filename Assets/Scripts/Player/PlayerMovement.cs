using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private Animator _animator;

        private MapManager _map;
        
        private bool _jump;
        private bool _climb;

        public float speed = 5f;
        public float crawlSpeed = 5f;
        public float jumpForce = 465f;

        // Start is called before the first frame update

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _map = GetComponent<MapManager>();
        }

        public void Jump()
        {
            _jump = true;
        }

        private void UpdateWalk()
        {
            Vector2 TargetVelocity;
            TargetVelocity = new Vector2(direction * speed, _rigidbody.velocity.y);
            
            
            Vector2 v = Vector2.zero;

            // PlayerClimbingAnimation();

            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, TargetVelocity, ref v, 0.06f);

            if (_jump)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce));
             
                _jump = false;
            }
        }
        
        private void UpdateClimb()
        {
            Vector2 TargetVelocity;
            TargetVelocity = new Vector2(_inpu * speed, crawlSpeed);
            ;
            
            
            Vector2 v = Vector2.zero;

            // PlayerClimbingAnimation();

            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, TargetVelocity, ref v, 0.06f);

            if (_jump)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce));

                _jump = false;
            }
        }
        
        private void FixedUpdate()
        {
            if (_climb)
            {
                UpdateClimb();
            }
            else
            {
                UpdateWalk();
            }
            
            //StartCoroutine(EndMotion()) ;
        }

        // void PlayerClimbingAnimation()
        // {
        //     RaycastHit2D leftTileHit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 0.42f, LayerMask.GetMask("Platform"));
        //     RaycastHit2D rightTileHit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 0.42f, LayerMask.GetMask("Platform"));
        //
        //     if (leftTileHit.collider && _crawlUp && player.GroundHeight >= transform.position.y)
        //     {
        //         player.ActiveLeftTileClimbingAnimation();
        //         player.ActiveClimbingAnimation();
        //     }
        //     else if (rightTileHit.collider && _crawlUp && player.GroundHeight >= transform.position.y)
        //     {
        //         player.ActiveRightTileClimbingAnimation();
        //         player.ActiveClimbingAnimation();
        //     }
        //     else if (_crawlUp && player.GroundHeight >= transform.position.y)
        //     {
        //         player.DisableClimbingAnimation();
        //         player.DisableTileClimbingAnimation();
        //         player.ActiveClimbingAnimation();
        //     }
        //     else if (!_crawlUp || player.GroundHeight < transform.position.y)
        //     {
        //         player.DisableClimbingAnimation();
        //         player.DisableTileClimbingAnimation();
        //     }
        // }
        // IEnumerator EndMotion()
        // {
        //     if (transform.position.y >= player.GroundHeight-0.5f)
        //     {
        //         player.transform.position = new Vector3(Mathf.Floor(player.transform.position.x), 2.0f, player.transform.position.z);
        //         player.ActiveLeftTileClimbingEndMotion();
        //     }
        //
        //     yield return new WaitForSeconds(2.0f);
        //     player.DisableLeftTileClimbingEndMotion();        
        // }
    }
}
