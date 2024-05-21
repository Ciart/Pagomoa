using System;
using System.Collections;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class MoaInteraction : MonoBehaviour
    {
        public Transform target;

        public float halfFloatingTime = 2f;
        public AnimationCurve moveCurve;

        private float _moaSpeed = 1;
        private bool _canInteraction;
        
        private int _floatingNormalized = -1;
        private bool _floatingEnd;
        
        private Vector2 _targetDirectionVector;
        private int _targetLastDirection = 1;
        private PlayerMovement _targetMovement;
        
        private const float LinearPoint = 1f;

        void Start()
        {
            StartCoroutine(FindPlayer());
        }
        
        void Update()
        {
            if(!target) return;

            if (_targetMovement) _targetDirectionVector = _targetMovement.directionVector;
            
            var destinationPos = target.position + new Vector3(1.5f * _targetLastDirection, 1f, 0);

            if (TargetIsNotMove() && _canInteraction)
            {
                if (!_floatingEnd) return;
                    
                if (!transform.parent)
                {
                    transform.SetParent(target);
                }
                    
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
                
                transform.position = Vector3.Lerp(startPos, endPos, moveCurve.Evaluate(percent));
                
                yield return null;
            }

            _floatingNormalized *= -1;
            _floatingEnd = true;
        }
        
            private IEnumerator FindPlayer()
        {
            yield return new WaitForSeconds(2f);

            var player = FindObjectOfType<PlayerMovement>();
            if (player)
            {
                target = player.transform;
                _targetMovement = player;
                _targetDirectionVector = player.directionVector;
            }
        }

        private bool TargetIsNotMove()
        {
            switch (_targetDirectionVector.x)
            {
                case 1 : _targetLastDirection = -1;
                    _canInteraction = false;
                    break;
                case -1 : _targetLastDirection = 1;
                    _canInteraction = false;
                    break;
            }

            return _targetDirectionVector.x == 0;
        }
        
        private void ChaseTarget(Vector3 destinationPos)
        {
            StopAllCoroutines();
            if (transform.parent) transform.SetParent(null);

            var distancePos = destinationPos - transform.position;

            _moaSpeed = (Mathf.Abs(distancePos.x) > 2 || Mathf.Abs(distancePos.y) > 2) ? 5f : 2f;

            transform.position = Vector3.MoveTowards(transform.position, destinationPos, Time.deltaTime * _moaSpeed);

            if (transform.position == destinationPos)
            {
                _floatingEnd = true;
                _canInteraction = true;
                _floatingNormalized = -1;
            }
        }
        
    }
}
