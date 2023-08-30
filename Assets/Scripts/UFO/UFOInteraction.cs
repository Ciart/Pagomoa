using System.Collections;
using Player;
using UnityEngine;


namespace UFO
{
    public class UFOInteraction : MonoBehaviour
    {
        public bool canMove = true;
        
        private Transform _player;

        private float _maxSpeed = 3f;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        }

        public IEnumerator MoveToPlayer()
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
            canMove = true;
        }
    }
}
