using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public interface IQuestEvent
    {
        public void CompleteEvent(string questId);
    }
    
    public class BrokenTent : MonoBehaviour, IQuestEvent
    {
        private const string RepairTentQuestID = "tutorial_3";
        
        [SerializeField] private GameObject tent;
        
        public void CompleteEvent(string questID)
        {
            if (RepairTentQuestID == questID)
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
