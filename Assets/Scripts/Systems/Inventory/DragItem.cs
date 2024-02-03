using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class DragItem : MonoBehaviour
    {
        static public DragItem Instance;

        void Start()
        {
            Instance = this;
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
