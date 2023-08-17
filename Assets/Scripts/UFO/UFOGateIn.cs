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

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player") isPlayer = true;
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Player") isPlayer = false;
        }
    }
}