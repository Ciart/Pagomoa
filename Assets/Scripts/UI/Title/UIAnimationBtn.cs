using UnityEngine;
using UnityEngine.EventSystems;

namespace Ciart.Pagomoa.UI.Title
{
    public class UIAnimationBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Animator anim;
        public void OnPointerEnter(PointerEventData eventData)
        {
            anim.SetBool("Play", true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            anim.SetBool("Play", false);
        }
    }
}
