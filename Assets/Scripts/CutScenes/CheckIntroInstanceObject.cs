using Ciart.Pagomoa.UI.Title;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class CheckIntroInstanceObject : MonoBehaviour
    {
        void Start()
        {
            TitleController.Instance.CheckIntroInstance(gameObject);        
        }
    }
}
