using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class MineralCollector 
    {
        public static Dictionary<string, int> minerals = new Dictionary<string, int>();
        
        public static event Action<string, int> OnMineralsChange;

        public static bool TryUseMineral(string mineralID, int count)
        {
            minerals.TryGetValue(mineralID, out var totalCount);
            if (totalCount < count) return false;

            SetMineralCount(mineralID, totalCount - count);
            return true;
        }

        public static void CollectMineral(string mineralID, int count)
        {
            if(minerals.TryGetValue(mineralID, out var totalCount))
                SetMineralCount(mineralID, totalCount + count);
            else
                minerals.Add(mineralID, count);
        }

        private static void SetMineralCount(string mineralID, int count)
        {
            if(minerals.TryGetValue(mineralID, out var totalCount))
                minerals[mineralID] = count;

            OnMineralsChange?.Invoke(mineralID, count);
        }

        public static int GetMineralCount(string mineralID)
        {
            if(minerals.TryGetValue(mineralID, out var totalCount))
                return totalCount;
            minerals.Add(mineralID, 0);
            return 0;
        }
    }
}
