using System;
using UnityEngine;

namespace Worlds
{
    [Serializable]
    public class WorldPrefab
    {
        public float x;

        public float y;

        public GameObject prefab;

        public WorldPrefab(float x, float y, GameObject prefab)
        {
            this.x = x;
            this.y = y;
            this.prefab = prefab;
        }
    }
}
