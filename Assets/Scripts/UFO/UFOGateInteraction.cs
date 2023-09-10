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
        public TilemapCollider2D gateFloor;

        public float floatSpeed = 10f;
        
        public int direction;

        private UFOGateIn _gateIn;
        private UFOGateOut _gateOut;

        private CircleCollider2D _inCollider;
        private CircleCollider2D _outCollider;

        private SpriteRenderer _beamBack;
        private SpriteRenderer _beamFront;

        private UFOInteraction _ufoInteraction;

        [SerializeField] private LayerMask _playerMask;

        private Vector2 _boxSize;
        
        private float _distance;

        private bool _beamActivate = false;

        void Start()
        {
            _ufoInteraction = GetComponentInParent<UFOInteraction>();
            
            gateFloor = transform.parent.Find("Grid").GetChild(2).GetComponent<TilemapCollider2D>();

            _gateIn = GetComponentInChildren<UFOGateIn>();
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

        private void ActiveGravityBeam()
        {
            if (_beamActivate) return ;

            StartCoroutine(nameof(MakeBeamZone));
        }

        private void SetDirectionY()
        {
            if (_gateIn.isPlayer) direction = 1;
            if (_gateOut.isPlayer) direction = -1;
        }

        private IEnumerator MakeBeamZone()
        {
            _beamActivate = true;
            
            _ufoInteraction.canMove = false;
            
            SetDirectionY();
            ControlDirectionTrigger(false);
            ActiveBeamSprites(true);
            
            float timer = 0f;
            Vector3 startPos = new Vector2(transform.position.x, transform.position.y - 13.9f);
            RaycastHit2D hit = Physics2D.BoxCast(startPos, _boxSize, 0f, Vector2.right, _distance, _playerMask);

            StartCoroutine(nameof(SetPhysics), hit);
            
            while (timer < 2f)
            {
                if (hit.collider)
                {
                    hit.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, direction * floatSpeed);
                }
                
                timer += Time.deltaTime;
                
                yield return null;
            }

            ControlDirectionTrigger(true);
            ActiveBeamSprites(false);
            
            _beamActivate = false;
            
            _ufoInteraction.canMove = true;
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

        private IEnumerator SetPhysics()
        {
            gateFloor.GetComponent<TilemapCollider2D>().enabled = false;
            
            yield return new WaitForSeconds(0.3f);

            gateFloor.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
