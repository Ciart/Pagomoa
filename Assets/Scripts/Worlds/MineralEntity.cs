using UnityEngine;

namespace Worlds
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MineralEntity : MonoBehaviour
    {
        public Mineral data;

        private SpriteRenderer _spriteRenderer;

        public Mineral Data
        {
            get => data;
            set
            {
                data = value;
                _spriteRenderer.sprite = data.sprite;
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteRenderer.sprite = data.sprite;
        }
    }
}