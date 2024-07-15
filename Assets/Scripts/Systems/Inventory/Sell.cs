using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Sell : MonoBehaviour
    {
        public static Sell Instance = null;

        [SerializeField] public Slot choiceSlot;
        [SerializeField] public ShopHover hovering;
        [SerializeField] private GameObject _slot;
        [SerializeField] private GameObject _slotParent;
        [SerializeField] private Sprite _image;
        private List<Slot> _slotDatas = new List<Slot>();
        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            MakeSlot();
        }
        private void OnEnable()
        {
            DeleteSlot();
            ResetSlot();
            transform.GetComponentInParent<ShopUIManager>().gold[0].text =GameManager.player.inventoryDB.Gold.ToString();
        }
        public void MakeSlot()
        {
            for(int i = 0; i <GameManager.player.inventoryDB.items.Length; i++)
            {
                GameObject SpawnedSlot = Instantiate(_slot, _slotParent.transform);
                _slotDatas.Add(SpawnedSlot.GetComponent<Slot>());
                _slotDatas[i].id = i;
                SpawnedSlot.SetActive(true);
            }
            ResetSlot();
        }
        public void ResetSlot()
        {
            for(int i = 0; i < _slotDatas.Count; i++)
                _slotDatas[i].inventoryItem = GameManager.player.inventoryDB.items[i];
            UpdateSlot();
        }
        public void UpdateSlot()
        {
            for (int i = 0; i < GameManager.player.inventoryDB.items.Length; i++)
            {
                string convert = GameManager.player.inventoryDB.items[i].count.ToString();
                if (GameManager.player.inventoryDB.items[i].count == 0)
                {
                    convert = "";
                }
                if (GameManager.player.inventoryDB.items[i].item == null)
                    _slotDatas[i].SetUI(_image, convert);
                else
                    _slotDatas[i].SetUI(GameManager.player.inventoryDB.items[i].item.itemImage, convert);
            }
        }
        public void DeleteSlot()
        {
            if (GameManager.player.inventoryDB.items.Length >= 0)
            {
                for (int i = 0; i < _slotDatas.Count; i++)
                    _slotDatas[i].SetUI(_image, "");
            }
        }
    }
}
