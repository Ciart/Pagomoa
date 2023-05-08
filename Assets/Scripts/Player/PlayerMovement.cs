using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerController player;
    
        private Rigidbody2D _rigidbody;

        private Animator _animator;

        public float direction;
        private bool _jump;
        private bool _crawlUp;

        public Transform groundCheck;
        
        public float speed = 5f;
        public float jumpForce = 465f;
        public float crawlSpeed = 5f;

        // Start is called before the first frame update

        void Awake()
        {
            player = GetComponent<PlayerController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void Jump()
        {
            _jump = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.C))
                _crawlUp = true;

            direction = Input.GetAxisRaw("Horizontal");
        }
        void FixedUpdate()
        {
            Vector2 TargetVelocity;
            TargetVelocity = _crawlUp && player.GroundHeight >= transform.position.y ? new Vector2(direction * speed, crawlSpeed) : new Vector2(direction * speed, _rigidbody.velocity.y);
            Vector2 v = Vector2.zero;

            PlayerClimbingAnimation();

            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, TargetVelocity, ref v, 0.06f);
            player.SetDirection(direction);

            if (_jump)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce));
             
                _jump = false;
            }

            _crawlUp = false;

            // Ÿ�� ��Ÿ�� ������ �ڵ�ƾ �Լ�
            //StartCoroutine(EndMotion()) ;
        }

        void PlayerClimbingAnimation()
        {
            RaycastHit2D leftTileHit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 0.42f, LayerMask.GetMask("Platform"));
            RaycastHit2D rightTileHit = Physics2D.Raycast(transform.position, new Vector2(1, 0), 0.42f, LayerMask.GetMask("Platform"));

            if (leftTileHit.collider && _crawlUp && player.GroundHeight >= transform.position.y)
            {
                player.ActiveLeftTileClimbingAnimation();
                player.ActiveClimbingAnimation();
            }
            else if (rightTileHit.collider && _crawlUp && player.GroundHeight >= transform.position.y)
            {
                player.ActiveRightTileClimbingAnimation();
                player.ActiveClimbingAnimation();
            }
            else if (_crawlUp && player.GroundHeight >= transform.position.y)
            {
                player.DisableClimbingAnimation();
                player.DisableTileClimbingAnimation();
                player.ActiveClimbingAnimation();
            }
            else if (!_crawlUp || player.GroundHeight < transform.position.y)
            {
                player.DisableClimbingAnimation();
                player.DisableTileClimbingAnimation();
            }
        }
        IEnumerator EndMotion()
        {
            if (transform.position.y >= player.GroundHeight-0.5f)
            {
                player.transform.position = new Vector3(Mathf.Floor(player.transform.position.x), 2.0f, player.transform.position.z);
                player.ActiveLeftTileClimbingEndMotion();
            }

            yield return new WaitForSeconds(2.0f);
            player.DisableLeftTileClimbingEndMotion();        
        }
    }
}
