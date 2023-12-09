using Cinemachine;
using Entities;
using UnityEngine;
using PlayerController = Entities.Players.PlayerController;

namespace Cameras
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