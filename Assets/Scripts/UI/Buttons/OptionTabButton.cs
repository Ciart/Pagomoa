using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa
{
    public class OptionTabButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _leftPointer;
        [SerializeField] private GameObject _rightPointer;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _leftPointer.SetActive(true);
            _rightPointer.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _leftPointer.SetActive(false);
            _rightPointer.SetActive(false);
        }
    }
}
