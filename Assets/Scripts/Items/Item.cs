using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Items
{
    public enum ItemType
    {
        None = 0,
        Equipment,
        Use,
        Mineral,
        Inherent
    }
    
    [Serializable]
    public class Item
    {
        public ItemType type = 0;

        public string id = "";

        public string name = "";

        public string description = "";

        public int price = 0;

        public PlayerStatusModifier? status = null;

        public Sprite? sprite = null;

        private NewItemEffect? _useEffect = null;

        private void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Items/{id}");
        }

        public void Init(NewItemEffect? useEffect = null)
        {
            _useEffect = useEffect;

            LoadResources();
        }

        public void DisplayUseEffect()
        {
            if (type != ItemType.Use) return;

            _useEffect?.Effect();
        }

        public void SetItem(Item item)
        {
            type = item.type;
            id = item.id;
            name = item.name;
            description = item.description;
            price = item.price;
            sprite = item.sprite;
            _useEffect = item._useEffect;
        }
        
        public void ClearItemProperty()
        {
            type = 0;
            id = "";
            name = string.Empty;
            description = string.Empty;
            price = 0;
            sprite = null;
            _useEffect = null;
        } 
    }
}
