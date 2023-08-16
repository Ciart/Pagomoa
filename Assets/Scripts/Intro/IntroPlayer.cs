using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Intro
{
    public class IntroPlayer : MonoBehaviour
    {
        public float speed;

        public void Move()
        {
            var pos = transform.position;
            Vector3 movePos = new Vector3(pos.x - (speed * Time.deltaTime), pos.y);
            transform.SetPositionAndRotation(movePos, transform.rotation); 
        }
    }    
}