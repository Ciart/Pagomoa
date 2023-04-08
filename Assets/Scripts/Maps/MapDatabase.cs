using UnityEngine;

namespace Maps
{
    [CreateAssetMenu(fileName = "Map Database", menuName = "Map/Database", order = 4)]
    public class MapDatabase : ScriptableObject
    {
        public Mineral[] minerals;

        public Ground[] grounds;

        public Piece[] pieces = {new Piece()};

        public int selectIndex;
    }
}