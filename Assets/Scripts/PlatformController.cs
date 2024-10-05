using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private EdgeCollider2D _collider;

    private PlayerInput _playerInput;

    private BoxCollider2D _playerCollider;

    private float _enableTime = 0f;

    private void OnPlayerSpawnedEvent(PlayerSpawnedEvent e)
    {
        _playerInput = e.player.GetComponent<PlayerInput>();
        _playerCollider = e.player.GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        _collider = GetComponent<EdgeCollider2D>();
    }

    private void OnEnable()
    {
        EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawnedEvent);
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