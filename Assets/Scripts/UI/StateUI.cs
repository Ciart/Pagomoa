using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class StateUI : MonoBehaviour
    {
        [SerializeField] private Image oxygenBar;
        [SerializeField] private TextMeshProUGUI goldText;
        
        // Todo : HP바 추가 해야함
        // Todo : Hunger바 추가 해야함

        public void UpdateOxygenBar(float currentOxygen, float maxOxygen)
        {
            oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }
        
        public void SetMoneyUI()
        {
            var inventory = GameManager.instance.player.inventory;
            
            goldText.text = inventory.Gold.ToString();
        }
    }
}
