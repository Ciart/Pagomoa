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

        private SpriteRenderer _beamBack;
        private SpriteRenderer _beamFront;
        
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

            _beamBack = transform.GetChild(3).GetComponent<SpriteRenderer>();
            _beamFront = transform.GetChild(4).GetComponent<SpriteRenderer>();
            _beamFront.enabled = false;
            _beamBack.enabled = false;
            
            _boxSize = new Vector2(1.5f, 20);
        }

        public void ActiveGravityBeam()
        {
            SetDirectionY();

            _startPos = new Vector2(transform.position.x, transform.position.y - 13.9f);
            var hit = Physics2D.BoxCast(_startPos, _boxSize, 0f, Vector2.right, _distance, _playerMask);
            
            if (hit.collider)
            {
                hit.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, direction * floatSpeed);
                
                if (direction == -1)
                {
                    Physics2D.IgnoreCollision( hit.transform.GetComponent<BoxCollider2D>(), UFOFloor.GetComponent<TilemapCollider2D>(), true);
                }
                
                if (!_beamFront.enabled) StartCoroutine(nameof(DisableFloor), hit);
            }
        }

        private void SetDirectionY()
        {
            if (_gateIn.isPlayer) direction = 1;
            if (_gateOut.isPlayer) direction = -1;
        }

        private IEnumerator DisableFloor(RaycastHit2D hit)
        {
            ControlDirectionTrigger(false);
            ActiveBeamSprites(true);

            yield return new WaitForSeconds(2f);
            
            Physics2D.IgnoreCollision( hit.transform.GetComponent<BoxCollider2D>(), UFOFloor.GetComponent<TilemapCollider2D>(), false);

            ControlDirectionTrigger(true);
            ActiveBeamSprites(false);
        }

        private void ActiveBeamSprites(bool enable)
        {
            _beamFront.enabled = enable;
            _beamBack.enabled = enable; 
        }

        private void ControlDirectionTrigger(bool enable)
        {
            _inCollider.enabled = enable;
            _outCollider.enabled = enable;
        }
        private void OnDrawGizmos()
        {
            // 시각적으로 박스 캐스트를 그리기 위해 Gizmos를 사용합니다.
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_startPos, _boxSize);
            Gizmos.DrawLine(_startPos, _startPos + Vector3.right * _distance);
        }
    }
}
