using System;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.UI
{
    public class MinimapUI : MonoBehaviour
    {
        [Header("플레이어 위치 UI")]
        [SerializeField] private OutputPlayerVector outputPlayerVectorX;
        [SerializeField] private OutputPlayerVector outputPlayerVectorY;
        
        [Header("플레이 타임 UI")]
        [SerializeField] private OutputPlayerPlayTime outputPlayDays;
        [SerializeField] private OutputPlayerPlayTime outputPlayTimeMinutes;

        private void Start()
        {
            outputPlayerVectorX.SetTargetVector(0);
            outputPlayerVectorY.SetTargetVector(1);
            
            outputPlayDays.SetTargetTimeValue(0);
            outputPlayTimeMinutes.SetTargetTimeValue(1);
        }

        public void UpdateMinimap()
        {
            outputPlayerVectorX.UpdateVectorOutput();
            outputPlayerVectorY.UpdateVectorOutput();

            outputPlayDays.UpdatePlayTimeOutput();
            outputPlayTimeMinutes.UpdatePlayTimeOutput();
        }
    }
}
