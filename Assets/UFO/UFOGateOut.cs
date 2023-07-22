using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace UFO
{
    public class UFOGateOut : MonoBehaviour
    {
        public bool isPlayer;
        public CircleCollider2D outCollider;
        private InteractableObject _interact;
        void Start()
        {
            _interact = GetComponent<InteractableObject>();
            outCollider = GetComponent<CircleCollider2D>();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player")
            {
                _interact.ActiveObject();
                isPlayer = true;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Player")
            {
                _interact.DisableObject();
                isPlayer = false;
            }
        }
    }
}