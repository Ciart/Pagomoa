using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intro
{
    public class IntroFirstFocus : MonoBehaviour
    {
        public float speed;
        private bool _sceneEnd;

        public void Move()
        {
            if (_sceneEnd) return ;

                var pos = transform.position;
            
            if (transform.position.y < 0f)
            {
                Vector3 setPos = new Vector3(pos.x, 0f);
                transform.SetPositionAndRotation(setPos, transform.rotation);
                
                _sceneEnd = !_sceneEnd;
            }
            else
            {
                Vector3 movePos = new Vector3(pos.x, pos.y - (speed * Time.deltaTime));
                transform.SetPositionAndRotation(movePos, transform.rotation);     
            }
        }
    }    
}