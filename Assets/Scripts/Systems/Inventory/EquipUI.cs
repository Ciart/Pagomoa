using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class EquipUI : MonoBehaviour
    {
        public void OnUI()
        {
            transform.gameObject.SetActive(true);
        }
        public void OffUI()
        {
            transform.gameObject.SetActive(false);
        }
    }
}
