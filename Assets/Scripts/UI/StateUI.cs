using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class StateUI : MonoBehaviour
    {
        [SerializeField] private Image _hungerBar;
        [SerializeField] private Image _hpBar;
        [SerializeField] private Image _oxygenBar;
        public TextMeshProUGUI playerGoldUI;

        public void UpdateHungerBar()
        {
            var player = Game.Instance.player;
            if (player)
            {
                _hungerBar.fillAmount = player.hungry / player.maxHungry;
            }
                
        }
        public void UpdateHpBar()
        {
            var player = Game.Instance.player;
            if( player )
                _hpBar.fillAmount = player.health / player.maxHealth;
        }
        public void UpdateOxygenBar()
        {
            var player = Game.Instance.player;
            if( player )
                _hpBar.fillAmount = player.health / player.maxHealth;
        }
    }
}
