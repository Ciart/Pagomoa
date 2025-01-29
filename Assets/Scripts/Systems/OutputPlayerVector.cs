using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems
{
    public class OutputPlayerVector : MonoBehaviour
    {
        private enum TargetVector
        {
            TargetX = 0,
            TargetY = 1,
        }
        
        private Vector2 _playerPos;
        private TargetVector _targetVector;
        public void SetTargetVector(int value) { _targetVector = (TargetVector)value; }
        
        private TextMeshProUGUI _tmpGuiText;

        private void Awake()
        {
            _tmpGuiText = GetComponentInChildren<TextMeshProUGUI>();    
        }

        public void UpdateVectorOutput()
        {
            var player = Game.instance.player;
            
            if (!player) return; 
            _playerPos = player.transform.position;

            if (_targetVector == TargetVector.TargetX)
            {
            
                if (Math.Round(_playerPos.x) >= 1)
                    _tmpGuiText.text = $"{_playerPos.x:+0}";
                else if(_playerPos.x < 1)
                    _tmpGuiText.text = $"{_playerPos.x:0}";
            }
            else if (_targetVector == TargetVector.TargetY)
            {
                if(Math.Round(_playerPos.y) >= 1)
                    _tmpGuiText.text = $"{_playerPos.y:+0}";
                else if(_playerPos.y < 1)
                    _tmpGuiText.text = $"{_playerPos.y:0}";
            }
        }
    }
}
