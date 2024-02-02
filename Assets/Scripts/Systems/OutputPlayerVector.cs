using System;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class OutputPlayerVector : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        [SerializeField] private TextMeshProUGUI _tmpGuiText;
        private Vector2 _playerPos;
        void Update()
        {
            SetPlayerVectorOutput();
        }

        public void SetPlayerVectorOutput()
        {
            if (!_player) { return; }
            _playerPos = _player.position;

            if (gameObject.name == "CoordX")
            {
            
                if (Math.Round(_playerPos.x) >= 1)
                    _tmpGuiText.text = string.Format("{0:+0}", _playerPos.x);
                else if(_playerPos.x < 1)
                    _tmpGuiText.text = string.Format("{0:0}", _playerPos.x);
            }
            else if (gameObject.name == "CoordY")
            {
                if(Math.Round(_playerPos.y) >= 1)
                    _tmpGuiText.text = string.Format("{0:+0}", _playerPos.y);
                else if(_playerPos.y < 1)
                    _tmpGuiText.text = string.Format("{0:0}", _playerPos.y);
            }
        }
    }
}
