using Ciart.Pagomoa.Entities;
using Cinemachine;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Systems.Cameras
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class PlayerTargetGroup: MonoBehaviour
    {
        private CinemachineTargetGroup _targetGroup;
        
        private void OnSpawnedPlayer(PlayerController player)
        {
            _targetGroup.AddMember(player.transform, 1, 0);
        }
        
        private void Awake()
        {
            _targetGroup = GetComponent<CinemachineTargetGroup>();
            
            EntityManager.instance.spawnedPlayer += OnSpawnedPlayer;
        }
    }
}