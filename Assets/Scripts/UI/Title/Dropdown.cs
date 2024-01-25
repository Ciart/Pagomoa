using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.UI.Title
{
    public class Dropdown : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<TMP_Dropdown>().value = OptionDB.instance.scale - 1;
        }
    }
}
