using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Entities;
using Entities.Players;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private EdgeCollider2D _collider;

    private PlayerInput _playerInput;

    private BoxCollider2D _playerCollider;

    private float _enableTime = 0f;

    private void Awake()
    {
        _collider = GetComponent<EdgeCollider2D>();

        EntityManager.instance.spawnedPlayer += (player) =>
        {
            _playerInput = player.GetComponent<PlayerInput>();
            _playerCollider = player.GetComponent<BoxCollider2D>();
        };
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