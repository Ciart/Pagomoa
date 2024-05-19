using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class MoaInteraction : MonoBehaviour
    {
        public Transform target; 
        
        void Start()
        {
            while (target)
            {
                target = FindObjectOfType<PlayerController>().transform;    
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
