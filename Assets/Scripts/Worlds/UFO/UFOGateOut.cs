using UnityEngine;

namespace Ciart.Pagomoa.Worlds.UFO
{
    public class UFOGateOut : MonoBehaviour
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