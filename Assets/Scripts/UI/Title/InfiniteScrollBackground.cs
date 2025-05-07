using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.UI.Title
{
    public class InfiniteScrollBackground : ScrollBackground
    {
        public bool stopScroll;
        public bool startIntro;

        public bool needScrollDown;
        public bool needSpeedUpScroll;
        private float _speedDecreaseRate;
        private float _speedIncreaseRate;

        [Header("Scroll Duration")] 
        public float decreaseDuration = 2.0f;
        public float increaseDuration = 5.0f;
        
        [Header("MaxMin Speed")] 
        [SerializeField] private float minSpeed = 0.0f;
        [SerializeField] private float maxSpeed = 150.0f;
        
        [SerializeField] private float scrollOffset;
        [SerializeField] private float moveLength;
        [SerializeField] private float currentMoveLength;

        void Start()
        {
            if (scrollOffset != 0.0f) currentMoveLength += scrollOffset;

            _speedDecreaseRate = (speed - minSpeed) / decreaseDuration;
            _speedIncreaseRate = (maxSpeed - minSpeed) / increaseDuration;
        }
        
        protected override void Scroll()
        {
            if (stopScroll) return;

            ChangeForIntro();
            
            var frameLength = moveDirection.y * (Time.deltaTime * speed);
            
            currentMoveLength += Mathf.Abs(frameLength);
            transform.position += new Vector3(0.0f, frameLength, 0.0f);

            ScrollUp();
            
            if (startIntro) return;
            
            ScrollDown();
        }

        private void ScrollDown()
        {
            if (moveDirection != Vector3.up) return;
            
            if (currentMoveLength >= moveLength)
            {
                var gotoInitPos = new Vector3(0.0f, moveLength, 0.0f);
                currentMoveLength = 0.0f;
                transform.position -= gotoInitPos;
            }
        }

        private void ScrollUp()
        {
            if (moveDirection != Vector3.down) return; 
            
            if (currentMoveLength >= moveLength)
            {
                var gotoInitPos = new Vector3(0.0f, moveLength, 0.0f);
                currentMoveLength = 0.0f;
                transform.position += gotoInitPos;
            }
        }

        private void ChangeForIntro()
        {
            IncreaseSpeed();
            
            if (startIntro) return; 
            
            DecreaseSpeed();
            
            if (speed <= minSpeed)
            {
                needScrollDown = false;
                needSpeedUpScroll = true;
                    
                startIntro = true;
                moveDirection = Vector3.down;
                
                currentMoveLength = Mathf.Abs(moveLength - currentMoveLength);

                StartCoroutine(StopScroll());
            }
        }
        
        private void DecreaseSpeed()
        {
            if (!needScrollDown) return;

            if (speed <= minSpeed)
            {
                needScrollDown = false;
            }
            
            speed -= _speedDecreaseRate * Time.deltaTime;
            speed = Mathf.Max(speed, minSpeed);
        }

        private void IncreaseSpeed()
        {
            if (!needSpeedUpScroll) return;
            
            if (speed >= maxSpeed)
            {
                needSpeedUpScroll = false;
            }
            
            speed += _speedIncreaseRate * Time.deltaTime;
            speed = Mathf.Min(speed, maxSpeed);
        }

        private IEnumerator StopScroll()
        {
            if (increaseDuration == 0) stopScroll = true;
            
            yield return new WaitForSeconds(increaseDuration);
            
            stopScroll = true;
        }
    }
}
