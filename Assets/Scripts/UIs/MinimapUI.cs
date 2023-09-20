using System;
using Player;
using TMPro;
using UnityEngine;

namespace UIs
{
    public class MinimapUI : MonoBehaviour
    {
        public TextMeshProUGUI coordXText;
        public TextMeshProUGUI coordYText;

        private PlayerController _player;
        
        private void Awake()
        {
            _player = GameManager.instance.player;
        }

        private void Update()
        {
            var playerPosition = _player.transform.position;

            UpdateCoordText(coordXText, playerPosition.x);
            UpdateCoordText(coordYText, playerPosition.y);
        }

        private void UpdateCoordText(TextMeshProUGUI textUI, float coord)
        {
            textUI.text = Math.Round(coord) >= 1 ? $"{coord:+0}" : $"{coord:0}";
        }
    }
}
