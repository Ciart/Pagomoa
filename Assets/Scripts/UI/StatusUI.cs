using System;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class StatusUI : MonoBehaviour
    {
        [SerializeField] private Image _hungerBar;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _oxygenBar;
        public TextMeshProUGUI playerGoldUI;

        public void UpdateHungerBar()
        {
            var player = Game.Instance.player;
            if ( player )
            {
                _hungerBar.fillAmount = player.Hunger / player.MaxHunger;
            }
        }
        public void UpdateHealthBar()
        {
            var player = Game.Instance.player;
            if( player )
                _healthBar.fillAmount = player.Health / player.MaxHealth;
        }
        public void UpdateOxygenBar()
        {
            var player = Game.Instance.player;
            if( player )
                _oxygenBar.fillAmount = player.Oxygen / player.MaxOxygen;
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;
            
            player.oxygenChanged += UpdateOxygenBar;
            player.hungerChanged += UpdateHungerBar;
            player.healthChanged += UpdateHealthBar;
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
