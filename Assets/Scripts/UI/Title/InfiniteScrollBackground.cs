using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.UI.Title
{
    public class InfiniteScrollBackground : ScrollBackground
    {
         public bool goToBackGround;
        
        public override void Scroll()
        {
            if (goToBackGround && speed >= 0f) speed -= 0.8f;
            else if (goToBackGround) return;
            
            transform.position += moveDirection * (Time.deltaTime * speed);

            if (moveDirection == Vector3.down && speed < 150f) speed += 0.4f;

            if (moveDirection == Vector3.down && transform.position.y <= -48.2f)
            {
                gameObject.transform.position = new Vector2(0,32.6f);
                return;
            }
            
            if (moveDirection == Vector3.down) return;
            
            if (moveDirection == Vector3.up &&transform.position.y >= 32.6f)
            {
                gameObject.transform.position = new Vector2(0,-48.2f);
            }

            if (startIntro && speed >= 0f) speed -= 0.01f;
            
            if (speed > 0f) return;
            
            StartCoroutine(GotoSpaceBackground());
        }

        private IEnumerator GotoSpaceBackground()
        {
            moveDirection = Vector3.down;
            
            yield return new WaitForSeconds(6f);

            goToBackGround = true;
        } 
    }
}
