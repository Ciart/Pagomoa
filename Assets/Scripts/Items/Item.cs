using System;
using Ciart.Pagomoa.Entities.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Items
{
    public enum MineralHungryAmount
    {
        Copper = 20,
           
    }
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
        
        /// <value>float</value>
        /// hungerAmount는 ItemType이 Mineral일때만 사용가능합니다.
        /// <remarks>ItemType이 Mineral이 아닌 경우 에러 로그를 반환합니다.</remarks>
        public float hungeyAmount
        {
            get
            {
                if (type == ItemType.Mineral)
                    switch (id)
                    {
                        case "Copper":
                            return (float)MineralHungryAmount.Copper;
                        default:
                            throw new InvalidOperationException($"{id}의 배고픔 값을 찾을 수 없습니다.");
                    }
                
                throw new InvalidOperationException("ItemType이 Mineral 타입이어야 사용가능한 프로퍼티 입니다.");
            }
        }

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

        public void DisplayUseEffect(string itemID)
        {
            if (type == ItemType.Mineral) MineralCollector.CollectMineral(id, 1);
            if (type != ItemType.Use) return;

            _useEffect?.Effect(itemID);
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
