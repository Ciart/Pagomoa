using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

[Serializable]
public class BrickJsonData
{
    public Wall[] walls;
    
    public Ground[] grounds;
    
    public Mineral[] minerals;
}


namespace Ciart.Pagomoa.Systems
{
    public class ResourceManager : PManager<ResourceManager>
    {
        public Dictionary<string, NewItemEffect> itemEffects = new();

        public Dictionary<string, Item> items = new();

        public Dictionary<string, Wall> walls = new();
        
        public Dictionary<string, Ground> grounds = new();
        
        public Dictionary<string, Mineral> minerals = new();

        private void LoadItems()
        {
            var text = Resources.Load<TextAsset>("Items");

            foreach (var item in JsonUtility.FromJson<ItemJsonData>(text.ToString()).data)
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
        
        private void LoadBricks()
        {
            var text = Resources.Load<TextAsset>("Bricks");
            var data = JsonUtility.FromJson<BrickJsonData>(text.ToString());

            foreach (var wall in data.walls)
            {
                walls.Add(wall.id, wall);
                wall.LoadResources();
            }

            foreach (var ground in data.grounds)
            {
                grounds.Add(ground.id, ground);
                ground.LoadResources();
            }

            foreach (var mineral in data.minerals)
            {
                minerals.Add(mineral.id, mineral);
                mineral.LoadResources();
                Debug.Log(new PropertyName(mineral.id));
            }
        }

        public void UseItem(Item item)
        {
            itemEffects[item.id].Effect();
        }

        public override void Awake()
        {
            LoadItems();
            LoadBricks();
        }
    }
}
