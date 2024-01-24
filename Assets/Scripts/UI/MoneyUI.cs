using Ciart.Pagomoa.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpGuiText;

        private void FixedUpdate()
        {
            SetMoneyUI();
        }

        private void SetMoneyUI()
        {
            _tmpGuiText.text = InventoryDB.Instance.Gold.ToString();
        }
    }
}
