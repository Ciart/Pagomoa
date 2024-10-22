using System.Collections;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds.UFO
{
    public class UFOBedInteraction : MonoBehaviour
    {
        private Transform _player;

        private InteractableObject _interactable;
        
        void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            
            _interactable.interactionEvent.AddListener(GotoBed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player") _player = collision.transform;
        }
        
        public void GotoBed()
        {
            if (Game.Get<TimeManager>().canSleep)
            {
                Game.Get<TimeManager>().Sleep();

                StartCoroutine(nameof(Sleeping));
            } else {
                Debug.Log("잘 시간이 아닙니다.");
            }
        }

        private IEnumerator Sleeping()
        {
            _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _player.GetComponent<PlayerMovement>().canMove = false;
            _player.GetComponent<SpriteRenderer>().enabled = false;
            
            yield return new WaitForSeconds(7f);
            
            _player.GetComponent<SpriteRenderer>().enabled = true;
            _player.GetComponent<PlayerMovement>().canMove = true;
        }
    }
}
