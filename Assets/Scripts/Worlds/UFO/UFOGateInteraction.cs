using System.Collections;
using Ciart.Pagomoa.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Ciart.Pagomoa.Worlds.UFO
{
    public class UFOGateInteraction : MonoBehaviour
    {
        public float floatSpeed = 10f;
        
        public int direction;

        private UFOGateIn _gateIn;
        private UFOGateOut _gateOut;

        private CircleCollider2D _inCollider;
        private CircleCollider2D _outCollider;

        private SpriteRenderer _beamBack;
        private SpriteRenderer _beamFront;

        private UFOInteraction _ufoInteraction;
        
        private TilemapCollider2D _gateFloor;

        [SerializeField] private LayerMask _playerMask;

        private Vector2 _boxSize;
        
        private float _distance;

        private bool _beamActivate = false;

        void Start()
        {
            _ufoInteraction = GetComponentInParent<UFOInteraction>();
            
            Grid tilemap = _ufoInteraction.GetComponentInChildren<Grid>();
            _gateFloor = tilemap.transform.GetComponentInChildren<GateFloor>().GetComponent<TilemapCollider2D>(); 

            _gateIn = GetComponentInChildren<UFOGateIn>();
            _gateOut = GetComponent<UFOGateOut>();

            _inCollider = _gateIn.GetComponent<CircleCollider2D>();
            _outCollider = GetComponent<CircleCollider2D>();

            _gateIn.GetComponent<InteractableObject>().interactionEvent.AddListener(ActiveGravityBeam);
            _gateOut.GetComponent<InteractableObject>().interactionEvent.AddListener(ActiveGravityBeam);

            _beamBack = GetComponentInChildren<BeamBack>().GetComponent<SpriteRenderer>();
            _beamFront = GetComponentInChildren<BeamFront>().GetComponent<SpriteRenderer>();
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
            _gateFloor.GetComponent<TilemapCollider2D>().enabled = false;
            
            yield return new WaitForSeconds(0.3f);

            _gateFloor.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
