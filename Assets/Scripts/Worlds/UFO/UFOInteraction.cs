using System.Collections;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems;
using UnityEngine;


namespace Ciart.Pagomoa.Worlds.UFO
{
    public class UFOInteraction : MonoBehaviour
    {
        public bool canMove = true;

        private float _maxSpeed = 3f;

        public IEnumerator MoveToPlayer()
        {
            var player = GameManager.player;
            var startPos = transform.position;
            var targetPos = new Vector3(player.transform.position.x, startPos.y);
            var sqrDistance = (targetPos - startPos).sqrMagnitude;
            
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
