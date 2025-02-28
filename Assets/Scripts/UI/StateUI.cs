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
        
        // Todo : HP바 추가 해야함
        // Todo : Hunger바 추가 해야함

        public void UpdateHungerBar(float currentHunger, float maxHunger)
        {
            _hungerBar.fillAmount = currentHunger / maxHunger;
        }
        public void UpdateHpBar(float currentHp, float maxHp)
        {
            _hpBar.fillAmount = currentHp / maxHp;
        }
        public void UpdateOxygenBar(float currentOxygen, float maxOxygen)
        {
            _oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }
    }
}
