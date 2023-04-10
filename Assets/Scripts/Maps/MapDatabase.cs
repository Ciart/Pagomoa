using System;
using UnityEngine;

namespace Maps
{
    [CreateAssetMenu(fileName = "Map Database", menuName = "Map/Database", order = 4)]
    public class MapDatabase : ScriptableObject
    {
        public Mineral[] minerals;

        public Ground[] grounds;

        public Piece[] pieces = { new Piece() };

        public int selectIndex;

        public Mineral GetMineral(string name)
        {
            return Array.Find(minerals, mineral => mineral.name == name);
        }
        
        public Ground GetGround(string name)
        {
            return Array.Find(grounds, ground => ground.name == name);
        }
    }
}