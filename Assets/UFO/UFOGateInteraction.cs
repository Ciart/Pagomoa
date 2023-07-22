using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UFO
{
    public class UFOGateInteraction : MonoBehaviour
    {
        public KeyCode eventKey = KeyCode.E;
        
        public float floatSpeed = 10f;
        
        public int direction;
        
        public bool getKey;
        
        private UFOGateIn _gateIn;
        private UFOGateOut _gateOut;
        
        [SerializeField] private LayerMask _playerMask;
        
        private Vector3 _startPos;
        
        private Vector2 _boxSize;
        
        private float _distance;
        
        
        
        void Start()
        {
            _gateIn = transform.GetChild(2).GetComponent<UFOGateIn>();
            _gateOut = GetComponent<UFOGateOut>();

            _boxSize = new Vector2(3, 10);
        }

        private void Update()
        {
            InputEventKey();
            
            _startPos = new Vector2(transform.position.x, transform.position.y - 7f);
            RaycastHit2D hit = Physics2D.BoxCast(_startPos, _boxSize, 0f, Vector2.right, _distance, _playerMask);

            if (hit && getKey)
            {
                ActiveGravityBeam(hit.transform);
            }
        }

        private Vector2 ActiveGravityBeam(Transform hit)
        {
            SetDirectionY();

            return hit.GetComponent<Rigidbody2D>().velocity = new Vector2(0, direction * floatSpeed);
        }

        private void SetDirectionY()
        {
            if (_gateIn.isPlayer)
            {
                direction = 1;
            }
            
            if (_gateOut.isPlayer)
            {
                direction = -1;
            }
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
        /*private void OnDrawGizmos()
        {
            // 시각적으로 박스 캐스트를 그리기 위해 Gizmos를 사용합니다.
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_startPos, _boxSize);
            Gizmos.DrawLine(_startPos, _startPos + Vector3.right * _distance);
        }*/
    }
}
