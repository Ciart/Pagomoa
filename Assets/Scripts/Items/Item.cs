using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Items
{
    public enum ItemType
    {
        Equipment,
        Use,
        Mineral,
        Inherent
    }

    [Serializable]
    public class Item
    {
        public ItemType type;

        public string id;

        public string name;

        public string description;

        public int price;

        public Sprite? sprite;

        private NewItemEffect? _useEffect;

        private void LoadResources()
        {
            sprite = Resources.Load<Sprite>($"Items/{id}");
        }

        public void Init(NewItemEffect? useEffect = null)
        {
            _useEffect = useEffect;

            LoadResources();
        }

        public void Use()
        {
            if (type != ItemType.Use) return;

            _useEffect?.Effect();
        }
    }
}
