using System.Collections;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class MoaInteraction : MonoBehaviour
    {
        public Transform target;
        public GameObject appearanceEffect;
        
        public float halfFloatingTime = 2f;
        public AnimationCurve floatingCurve;
        
        [SerializeField] private float moaSpeed;
        [SerializeField] private float minSpeed;
        
        private InteractableObject _interactableObject;
        private bool _canInteraction;
        
        private int _floatingNormalized = -1;
        private bool _floatingEnd;
        
        private Vector2 _targetDirectionVector;
        private int _targetLastDirection = 1;
        private PlayerMovement _targetMovement;
        private bool _wasDigging;

        private const float LinearPoint = 1f;
        
        private SpriteRenderer _spriteRenderer;

        void Start()
        {
            _interactableObject = GetComponent<InteractableObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        void Update()
        {
            if (!target)
            {
                StopAllCoroutines();
                return;
            }

            if (!target.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                return;
            }

            if (_targetMovement) _targetDirectionVector = _targetMovement.directionVector;
            
            var destinationPos = target.position + new Vector3(1.5f * _targetLastDirection, 1f, 0);

            if (TargetIsNotMove() && _canInteraction)
            {
                if (!_floatingEnd) return;
                    
                if (!transform.parent) transform.SetParent(target);

                _floatingEnd = false;
                StartCoroutine(MoaFloating());
            }
            else
            {
                ChaseTarget(destinationPos);
            }
        }

        public void MoaTempInteraction() { Debug.Log(" 상호작용 "); }

        private IEnumerator MoaFloating()
        {
            var current = 0f;
            var percent = 0f;

            var startPos = transform.position;
            var endPos = _floatingNormalized == -1
                ? startPos + new Vector3(0, -LinearPoint, 0)
                : startPos + new Vector3(0, LinearPoint, 0);

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / halfFloatingTime;
                
                transform.position = Vector3.Lerp(startPos, endPos, floatingCurve.Evaluate(percent));
                
                yield return null;
            }

            _floatingNormalized *= -1;
            _floatingEnd = true;
        }
        
        public void InitMoa()
        {
            var player = FindAnyObjectByType<PlayerMovement>();
            if (!player.gameObject) return;
            
            target = player.transform;
            _targetMovement = player;
            _targetDirectionVector = player.directionVector;

            transform.SetParent(target);
        }

        private bool TargetIsNotMove()
        {
            switch (_targetDirectionVector.x)
            {
                case 1 : 
                    _targetLastDirection = -1;
                    _canInteraction = false;
                    _interactableObject.LockInteraction();
                    break;
                case -1 : 
                    _targetLastDirection = 1;
                    _canInteraction = false;
                    _interactableObject.LockInteraction();
                    break;
            }

            return _targetDirectionVector.x == 0;
        }
        
        private void ChaseTarget(Vector3 destinationPos)
        {
            StopAllCoroutines();
            
            if (transform.parent) transform.SetParent(null);

            var distancePos = destinationPos - transform.position;
            
            var xDistance = Mathf.Abs(distancePos.x);
            var yDistance = Mathf.Abs(distancePos.y);
            _spriteRenderer.flipX = distancePos.x > 0;    
            if (moaSpeed > 10f && (xDistance < 10f || yDistance < 10f)) moaSpeed += 0.02f;
        
            if (xDistance > 2f || yDistance > 2f) moaSpeed += 0.02f;
            else moaSpeed = minSpeed;
            
            transform.position = Vector3.Lerp(transform.position, destinationPos, Time.deltaTime * moaSpeed);

            if (Mathf.Abs(transform.position.x) <= Mathf.Abs(destinationPos.x) + 0.05f || Mathf.Abs(transform.position.y) <= Mathf.Abs(destinationPos.y) + 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinationPos, Time.deltaTime);
            }

            if (transform.position != destinationPos) return;

            moaSpeed = minSpeed;
                
            _floatingEnd = true;
            _floatingNormalized = -1;
                
            _canInteraction = true;
            _interactableObject.UnlockInteraction();
        }

        public void MoaAppearEffect()
        {
            StopAllCoroutines();
            
            if (transform.parent) transform.SetParent(null);
            
            var appearPoint = target.position + new Vector3(1.5f * _targetLastDirection, 1f, 0);
            
            GameObject activatedEffect = Instantiate(appearanceEffect, appearPoint, Quaternion.identity);
            Destroy(activatedEffect, 0.5f);

            transform.position = appearPoint;
            
            _floatingEnd = true;
            _floatingNormalized = -1;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
