using UnityEngine;

namespace Ciart.Pagomoa.Environments
{
    /// <summary>
    /// https://blog.yarsalabs.com/parallax-effect-in-unity-2d/
    /// </summary>
    public class ParallaxEffect : MonoBehaviour
    {
        public float amount = 0.1f;
        
        private Camera _mainCamera;
        private float _startingPos;
        private float _lengthOfSprite;


        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _startingPos = transform.position.x;
            _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x * 3, GetComponent<SpriteRenderer>().size.y);
        }


        private void Update()
        {
            var position = _mainCamera.transform.position;
            var temp = position.x * (1 - amount);
            var distance = position.x * amount;

            transform.position = new Vector3(_startingPos + distance, transform.position.y, transform.position.z);

            if (temp > _startingPos + (_lengthOfSprite / 2))
            {
                _startingPos += _lengthOfSprite;
            }
            else if (temp < _startingPos - (_lengthOfSprite / 2))
            {
                _startingPos -= _lengthOfSprite;
            }
        }
    }
}