using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        public Dictionary<string, NewItemEffect> itemEffects = new ();
        
        public Dictionary<string, Item> items = new ();
        
        private void LoadItems()
        {
            var text = Resources.Load<TextAsset>("Items");

            foreach (var item in JsonUtility.FromJson<JsonArrayData<Item>>(text.ToString()).data)
            {
                items.Add(item.id, item);
                item.LoadResources();
            }
            
            foreach(var assembly in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (assembly.IsSubclassOf(typeof(NewItemEffect)))
                {
                    var effect = Activator.CreateInstance(assembly) as NewItemEffect;
                    itemEffects.Add(assembly.Name.Replace("Effect", ""), effect);
                    
                    Debug.Log(assembly.Name.Replace("Effect", ""));
                }
            }
        }
        
        public void UseItem(Item item)
        {
            itemEffects[item.id].Effect();
        }

        protected override void Awake()
        {
            base.Awake();
            
            LoadItems();
        }
    }
}