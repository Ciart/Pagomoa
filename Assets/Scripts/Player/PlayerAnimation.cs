using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
        }
        void Start()
        {
        
        }

        void Update()
        {
            if (_playerMovement.direction == 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                animator.SetBool("goLeft", false);
            } 
            else if (_playerMovement.direction == 0 && Input.GetKeyUp(KeyCode.RightArrow))
            {
                animator.SetBool("goRight", false);
            }
            else if (_playerMovement.direction == -1)
            {
                animator.SetBool("goRight", false);
                animator.SetBool("goLeft", true);
            }
            else if (_playerMovement.direction == 1)
            {
                animator.SetBool("goLeft", false);
                animator.SetBool("goRight", true);
            }
        }
        void FixedUpdate()
        {
        
        }
    }
}
