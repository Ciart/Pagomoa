using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ResourceManager
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
            
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.Contains("Assembly-CSharp")).ExportedTypes)
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
        
        public ResourceManager(Game game)
        {
            game.awake += OnAwake;
        }

        private void OnAwake()
        {
            LoadItems();
        }
    }
}