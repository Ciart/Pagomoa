using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class TestCS : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Game.Instance.Entity.Spawn("Snake", transform.position);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
