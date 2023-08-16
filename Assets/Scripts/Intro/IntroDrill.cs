using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intro
{
    public class IntroDrill : MonoBehaviour
    {
        public float speed;
        private bool _sceneEnd;

        public void Fallen()
        {
            if (_sceneEnd) return ;
            
            var pos = transform.position;
            
            if (transform.position.y < 0.6f)
            {
                Vector3 setPos = new Vector3(pos.x, 0.5f);
                transform.SetPositionAndRotation(setPos, Quaternion.Euler(0, 0, -45f));

                _sceneEnd = !_sceneEnd;
            }
            else
            {
                Vector3 movePos = new Vector3(pos.x, pos.y - (speed * Time.deltaTime));
                transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
                transform.SetPositionAndRotation(movePos, transform.rotation);
            }
        }
    }    
}

