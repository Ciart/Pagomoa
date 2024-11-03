using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpGuiText;
        private void Start()
        {
            SetMoneyUI();
        }
        private void SetMoneyUI()
        {
            var inventory = GameManager.instance.player.inventory;
            
            _tmpGuiText.text = inventory.Gold.ToString();
        }
    }
}
