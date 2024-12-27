using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class DragItem : MonoBehaviour
    {
        public static DragItem instance;

        void Start()
        {
            instance = this;
        }
        public void DragSetImage(Sprite image)
        {
            this.transform.SetAsLastSibling();
            gameObject.GetComponent<Image>().sprite = image;
            SetColor(230);
        }    
        public void SetColor(float a)
        {
            Color color = gameObject.GetComponent<Image>().color;
            color.a = a;
            gameObject.GetComponent<Image>().color = color;
        }
    }
}
