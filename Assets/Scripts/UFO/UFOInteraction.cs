using System.Collections;
using Player;
using UnityEngine;


namespace UFO
{
    public class UFOInteraction : MonoBehaviour
    {
        private Transform _player;

        private float _maxSpeed = 3f;

        private bool _canMove = true;
        
        private void Start()
        {
            Debug.Log("B키를 누르면 UFO가 플레이어 위치로 소환됩니다.");
            _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        }
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.B) && _canMove)
            {
                _canMove = false;
                StartCoroutine(MoveToPlayer());
            }
        }
        
        private IEnumerator MoveToPlayer()
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = new Vector3(_player.position.x, startPos.y);
            float sqrDistance = (targetPos - startPos).sqrMagnitude;
            
            while (sqrDistance > 0.01f)
            {
                if (sqrDistance < 26f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, (_maxSpeed-Time.deltaTime) * Time.deltaTime);
                    sqrDistance = (targetPos - transform.position).sqrMagnitude;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 20f * Time.deltaTime);
                    sqrDistance = (targetPos - transform.position).sqrMagnitude;    
                }
                
                yield return null;
            }
            _canMove = true;
        }
    }
}
