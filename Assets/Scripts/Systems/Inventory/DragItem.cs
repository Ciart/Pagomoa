using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class DragItem : MonoBehaviour
    {
        public static DragItem instance;

        [SerializeField] private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        public void DragSetImage(Sprite image)
        {
            transform.SetAsLastSibling();
            _image.sprite = image;
            SetColor(200);
        }    
        public void SetColor(float a)
        {
            Color color = _image.color;
            color.a = a;
            _image.color = color;
        }
    }
}
