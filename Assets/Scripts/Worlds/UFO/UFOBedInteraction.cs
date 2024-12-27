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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player") _player = collision.transform;
        }
        
        private IEnumerator Sleeping()
        {
            _player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            _player.GetComponent<PlayerMovement>().canMove = false;
            _player.GetComponent<SpriteRenderer>().enabled = false;
            
            yield return new WaitForSeconds(7f);
            
            _player.GetComponent<SpriteRenderer>().enabled = true;
            _player.GetComponent<PlayerMovement>().canMove = true;
        }
    }
}
