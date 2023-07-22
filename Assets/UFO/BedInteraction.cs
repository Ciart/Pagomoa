using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UFO
{
    public class BedInteraction : MonoBehaviour
    {
        public KeyCode eventKey = KeyCode.E;

        public bool getKey;
        
        private InteractableObject _interact;
        
        
        
        void Start()
        {
            _interact = GetComponent<InteractableObject>();
        }
        
        void Update()
        {
            InputEventKey();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            _interact.ActiveObject();
            if (getKey && collision.name == "Player")
            {
                collision.GetComponent<PlayerMovement>().canMove = false;
                // 타임 매니저
                // 슬립모드
                // 플레이어 움직임 제어
                // 시간 스킵
                // 플레이어 슬립 애니메이션
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            _interact.DisableObject();
        }
        
        private void InputEventKey()
        {
            if (Input.GetKey(eventKey))
            {
                getKey = true;
            }
            else
            {
                getKey = false;
            }
        }
    }
}