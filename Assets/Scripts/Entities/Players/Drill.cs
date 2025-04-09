using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa
{

    [CreateAssetMenu(fileName = "Drill Data", menuName = "Drill/Drill Data", order = int.MaxValue)]
    public class Drill : ScriptableObject
    {
        public int tier;
        public int speed;
        public DrillUpgradeNeeds[] upgradeNeeds;
        public string drillName;
        public string drillDescription;
    }

    [Serializable]
    public class DrillUpgradeNeeds
    {
        public Item mineral;
        public int count;
    }
}
