using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class PlayerGetHit : MonoBehaviour
    {
        public bool isInvisible;

        [SerializeField] private float _invisibleCooltime = 2f;

        [SerializeField] private float _knockBackForce = 5f;

        private Status _status;

        private Rigidbody2D _playerRigidbody2D;

        private PlayerMovement _playerMovement;

        private PlayerController _playerController;

        private void Awake()
        {
            _playerRigidbody2D = GetComponent<Rigidbody2D>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerController = GetComponent<PlayerController>();
        }

        public IEnumerator InvincibleCool(Vector3 monsterPosition)
        {
            isInvisible = true;
            _playerMovement.canMove = false;

            KnockBack(monsterPosition);
            
            InvokeRepeating("SetCanMove", 0.05f, 0.05f);            

            yield return new WaitForSeconds(_invisibleCooltime);
            
            isInvisible = false;
            _playerMovement.canMove = true;
        }

        private void KnockBack(Vector3 monsterPosition)
        {
            Vector2 knockBackDirection = transform.position - monsterPosition;
            knockBackDirection.Normalize();
            Vector2 knockBackPosition = new Vector2(_knockBackForce * Mathf.Sign(knockBackDirection.x), 10f);
            
            _playerRigidbody2D.AddForce(knockBackPosition, ForceMode2D.Impulse);
        }

        private void SetCanMove()
        {
            if (_playerController.isGrounded)
            {
                _playerMovement.canMove = true;
            }
        }
    }
}