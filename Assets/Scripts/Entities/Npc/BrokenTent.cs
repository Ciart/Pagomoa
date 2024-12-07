using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public interface IQuestEvent
    {
        public void CompleteEvent();
    }
    
    public class BrokenTent : MonoBehaviour, IQuestEvent
    {
        [SerializeField] private GameObject tent;
        
        

        public void CompleteEvent()
        {
            RepairTent();
        }
            
        private void RepairTent()
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            
            Instantiate(tent, transform.position, Quaternion.identity);
            
            gameObject.SetActive(false);
        }
    }
}
