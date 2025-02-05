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

        private PlayerInput _playerInput;

        private BoxCollider2D _playerCollider;

        private float _enableTime = 0f;
        
        private void SetPlayerComponent(PlayerController player)
        {
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

            if (Game.instance.player != null)
            {
                SetPlayerComponent(Game.instance.player);
            }
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawnedEvent);
        }

        private void Update()
        {
            if (_playerInput && DirectionUtility.ToDirection(_playerInput.Move) == Direction.Down)
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
