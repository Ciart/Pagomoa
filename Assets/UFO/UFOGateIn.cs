using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UFO
{
    public class UFOGateIn : MonoBehaviour
    {
        public bool isPlayer;
        public CircleCollider2D inCollider;
        private InteractableObject _interact;
        void Start()
        {
            _interact = GetComponent<InteractableObject>();
            inCollider = GetComponent<CircleCollider2D>();
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