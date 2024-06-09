using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{

    [CreateAssetMenu(fileName = "Drill Data", menuName = "Drill/Drill Data", order = int.MaxValue)]
    public class Drill : ScriptableObject
    {
        public int tier;
        public int speed;
        public DrillUpgradeNeeds[] upgradeNeeds;
    }

    [Serializable]
    public class DrillUpgradeNeeds
    {
        public MineralItem mineral;
        public int count;
    }
}
