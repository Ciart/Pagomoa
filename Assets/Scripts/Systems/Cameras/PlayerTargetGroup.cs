using System;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Cinemachine;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Systems.Cameras
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class PlayerTargetGroup: MonoBehaviour
    {
        private CinemachineTargetGroup _targetGroup;
        
        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            _targetGroup.AddMember(e.player.transform, 1, 0);
        }
        
        private void Awake()
        {
            _targetGroup = GetComponent<CinemachineTargetGroup>();
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
    }
}