using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    [Serializable]
    public struct Copper
    {
        public int Count;
    }
    [Serializable]
    public struct Iron
    {
        public int Count;
    }
public class Count : MonoBehaviour
{
    public Copper copperCount;
    public Iron ironCount;
}
