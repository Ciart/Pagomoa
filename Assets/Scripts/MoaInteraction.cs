using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa
{
    public class MoaInteraction : MonoBehaviour
    {
        public Transform target;

        public float halfFloatingTime = 2f;
        public AnimationCurve moveCurve;

        private InteractableObject _interactable;
        
        private int _floatingNormalized = -1;
        private bool _floatingEnd;
        private Vector3 _prevTargetPosition;

        private const float LinearPoint = 1f;
        
        
        

        void Start()
        {
            StartCoroutine(FindPlayer());

            _interactable = GetComponent<InteractableObject>();
        }
        
        void Update()
        {
            if(!target) return;
            
            if (TargetIsNotMove() && _floatingEnd)
            {
                // todo: interaction 가능 & 상 하 선형운동
                _floatingEnd = false;
                StartCoroutine(MoaFloating());
            }
            else
            {
                // todo: interaction 불가능 & 플레이어 따라가기 이건 Vector2 선형
                StopAllCoroutines();
                _floatingEnd = true;
                
                transform.SetParent(null);

                var targetDirection = (_prevTargetPosition - target.position).x > 0 ? -1 : 1;    
                var directionPos = _prevTargetPosition - transform.position;
                
                transform.position += directionPos * Time.deltaTime;
            }

            _prevTargetPosition = target.position;
             
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

            var moaTransform = transform;
            
            target = FindObjectOfType<PlayerController>().transform;
            
            moaTransform.SetParent(target);
            moaTransform.position = target.position + new Vector3(1.5f, 1f, 0);

            _floatingEnd = true;
        }

        private bool TargetIsNotMove()
        {
            return _prevTargetPosition == target.position;
        }
        
    }
}
