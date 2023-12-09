using System;
using Entities;
using TMPro;
using UnityEngine;

namespace UIs
{
    public class MinimapUI : MonoBehaviour
    {
        public TextMeshProUGUI coordXText;
        public TextMeshProUGUI coordYText;
        
        private void Update()
        {
            var position = EntityManager.instance.player.transform.position;

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
