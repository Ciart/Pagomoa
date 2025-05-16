using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class UIAnimationBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private Sprite _defaultSprite;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _anim.SetBool("Play", true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            _anim.SetBool("Play", false);
            GetComponent<Image>().sprite = _defaultSprite;
        }
    }
}
