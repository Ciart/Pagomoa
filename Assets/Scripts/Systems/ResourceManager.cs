using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        public Dictionary<string, NewItemEffect> itemEffects = new();

        public Dictionary<string, Item> items = new();

        public Dictionary<string, Wall> walls = new();
        
        public Dictionary<string, Ground> grounds = new();
        
        public Dictionary<string, Mineral> minerals = new();

        private void LoadItems()
        {
            var text = Resources.Load<TextAsset>("Items");

            foreach (var item in JsonUtility.FromJson<JsonArrayData<Item>>(text.ToString()).data)
            {
                items.Add(item.id, item);
                item.LoadResources();
            }

            foreach (var assembly in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (assembly.IsSubclassOf(typeof(NewItemEffect)))
                {
                    var effect = Activator.CreateInstance(assembly) as NewItemEffect;
                    itemEffects.Add(assembly.Name.Replace("Effect", ""), effect);

                    Debug.Log(assembly.Name.Replace("Effect", ""));
                }
            }
        }
        
        private void LoadWalls()
        {
            var text = Resources.Load<TextAsset>("Walls");

            foreach (var wall in JsonUtility.FromJson<JsonArrayData<Wall>>(text.ToString()).data)
            {
                walls.Add(wall.id, wall);
                wall.LoadResources();
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