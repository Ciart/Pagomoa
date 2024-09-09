using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Utilities
{
    public static class PrefabUtility
    {
        public static void ResizeParentList<T>(List<T> list, GameObject parent, T prefab, int count,
            Action<T> init = null) where T : Object
        {
            if (list.Count < count)
            {
                for (var i = list.Count; i < count; i++)
                {
                    var item = Object.Instantiate(prefab, parent.transform);
                    
                    init?.Invoke(item);
                    
                    list.Add(item);
                }
            }
            else if (list.Count > count)
            {
                for (var i = list.Count - 1; i >= count; i--)
                {
                    var item = list[i];
                    list.RemoveAt(i);
                    
                    Object.Destroy(item.GameObject());
                }
            }
        }
    }
}