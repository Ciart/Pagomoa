using System;
using UnityEngine;

namespace Worlds
{
    [CreateAssetMenu(fileName = "World Database", menuName = "World/Database", order = 4)]
    public class WorldDatabase : ScriptableObject
    {
        public Wall[] walls;

        public Ground[] grounds;

        public Mineral[] minerals;

        public Piece[] pieces = { new Piece() };

        public int selectIndex;
        
        public Wall GetWall(string name)
        {
            return Array.Find(walls, wall => wall.name == name);
        }

        public Ground GetGround(string name)
        {
            return Array.Find(grounds, ground => ground.name == name);
        }
        
        public Mineral GetMineral(string name)
        {
            return Array.Find(minerals, mineral => mineral.name == name);
        }
    }
}