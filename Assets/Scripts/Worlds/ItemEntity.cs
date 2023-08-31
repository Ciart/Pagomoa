using UnityEngine;

namespace Worlds
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemEntity: MonoBehaviour
    {
<<<<<<< HEAD
<<<<<<< HEAD
        public Item item;
=======
        private Item _item;
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
        private Item _item;
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
    
        private SpriteRenderer _spriteRenderer;

        public Item Item
        {
<<<<<<< HEAD
<<<<<<< HEAD
            get => item;
            set
            {
                item = value;
                _spriteRenderer.sprite = item.itemImage;
=======
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
            get => _item;
            set
            {
                _item = value;
                _spriteRenderer.sprite = _item.itemImage;
<<<<<<< HEAD
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
            }
        }
    
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
<<<<<<< HEAD
<<<<<<< HEAD

            _spriteRenderer.sprite = item.itemImage;
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
=======
>>>>>>> 7e9333594d5bd1dc77a4bd024bf3d59a0e732d85
        }
    }
}