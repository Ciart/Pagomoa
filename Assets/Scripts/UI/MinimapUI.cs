using System;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.UI
{
    public class MinimapUI : MonoBehaviour
    {
        public TextMeshProUGUI coordXText;
        public TextMeshProUGUI coordYText;

        private void Update()
        {
            var position = GameManager.instance.player.transform.position;

            if (Math.Round(position.x) >= 1)
                coordXText.text = $"{position.x:+0}";
            else if(position.x < 1)
                coordXText.text = $"{position.x:0}";

            if(Math.Round(position.y) >= 1)
                coordYText.text = $"{position.y:+0}";
            else if(position.y < 1)
                coordYText.text = $"{position.y:0}";
        }
    }
}
