using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Achievements : MonoBehaviour
    {
        public List<InventorySlot> AchieveMinerals = new List<InventorySlot>();

        private static Achievements instance;
        public static Achievements Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType(typeof(Achievements)) as Achievements;
                }
                return instance;
            }
        }
    }
}
