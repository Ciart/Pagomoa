using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UFO
{
    public class UFOGateInteraction : MonoBehaviour
    {
        public TilemapCollider2D UFOFloor;

        public float floatSpeed = 10f;
        
        public int direction;

        private UFOGateIn _gateIn;
        private UFOGateOut _gateOut;

        private CircleCollider2D _inCollider;
        private CircleCollider2D _outCollider;
        
        [SerializeField] private LayerMask _playerMask;
        
        private Vector3 _startPos;
        
        private Vector2 _boxSize;
        
        private float _distance;

        void Start()
        {
            UFOFloor = transform.parent.Find("Grid").GetChild(0).GetComponent<TilemapCollider2D>();

            _gateIn = transform.GetChild(2).GetComponent<UFOGateIn>();
            _gateOut = GetComponent<UFOGateOut>();

            _inCollider = _gateIn.GetComponent<CircleCollider2D>();
            _outCollider = GetComponent<CircleCollider2D>();

            _gateIn.GetComponent<InteractableObject>().InteractionEvent.AddListener(ActiveGravityBeam);
            _gateOut.GetComponent<InteractableObject>().InteractionEvent.AddListener(ActiveGravityBeam);
            
            _boxSize = new Vector2(1, 8);
        }

        public void ActiveGravityBeam()
        {
            SetDirectionY();
            if ( direction < 0 ) UFOFloor.enabled = false;
            
            _startPos = new Vector2(transform.position.x, transform.position.y - 7.9f);
            var hit = Physics2D.BoxCast(_startPos, _boxSize, 0f, Vector2.right, _distance, _playerMask);
            
            if (hit.collider)
            {
                hit.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, direction * floatSpeed);
                
                _inCollider.enabled = false;
                _outCollider.enabled = false;
                
                StartCoroutine(nameof(DisableFloor));
            }
        }

        private void SetDirectionY()
        {
            if (_gateIn.isPlayer) direction = 1;
            if (_gateOut.isPlayer) direction = -1;
        }

        private IEnumerator DisableFloor()
        {
            yield return new WaitForSeconds(2f);
            
            _inCollider.enabled = true;
            _outCollider.enabled = true;
            UFOFloor.enabled = true;    
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
