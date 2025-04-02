using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class PlatformController : MonoBehaviour
    {
        private CompositeCollider2D _collider;

        private PlayerController _player;

        private PlayerInput _playerInput;

        private BoxCollider2D _playerCollider;

        private float _enableTime = 0f;
        
        private void SetPlayerComponent(PlayerController player)
        {
            _player = player;
            _playerInput = player.GetComponent<PlayerInput>();
            _playerCollider = player.GetComponent<BoxCollider2D>();
        }
        
        private void OnPlayerSpawnedEvent(PlayerSpawnedEvent e)
        {
            SetPlayerComponent(e.player);
        }

        private void Awake()
        {
            _collider = GetComponent<CompositeCollider2D>();
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawnedEvent);

            if (Game.Instance.player != null)
            {
                SetPlayerComponent(Game.Instance.player);
            }
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawnedEvent);
        }

        private bool CheckPassingPlayer()
        {
            if (_player == null || _playerInput == null)
            {
                return false;
            }

            if (DirectionUtility.ToDirection(_playerInput.Move) == Direction.Down)
            {
                return true;
            }

            var drill = _player.drill;

            if (drill.isDig && drill.isGroundHit && drill.direction == Direction.Down)
            {
                return true;
            }

            return false;
        }

        private void Update()
        {
            if (_playerInput && CheckPassingPlayer())
            {
                _enableTime = 0.5f;
            }
            else
            {
                _enableTime -= Time.deltaTime;
            }

            if (_enableTime >= 0f)
            {
                Physics2D.IgnoreCollision(_playerCollider, _collider);
            }
            else
            {
                Physics2D.IgnoreCollision(_playerCollider, _collider, false);
            }
        }
    }
}
