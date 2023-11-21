using System;
using System.Collections.Generic;
using UnityEngine;
using Worlds;

namespace Logger
{
    [Serializable]
    public class ConditionType
    {
        public TargetType Target { get; set; }
        public string TypeValue { get; set; }
    }
}
