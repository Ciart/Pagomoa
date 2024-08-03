using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Sell : MonoBehaviour
    {
        public static Sell Instance = null;

        [SerializeField] public InventorySlotUI choiceSlot;
        [SerializeField] public ShopHover hovering;
        [SerializeField] private GameObject _slot;
        [SerializeField] private GameObject _slotParent;
        [SerializeField] private Sprite _image;
        private List<InventorySlotUI> _slotDatas = new List<InventorySlotUI>();
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
            transform.GetComponentInParent<ShopUIManager>().gold[0].text =GameManager.player.inventory.Gold.ToString();
        }
        public void MakeSlot()
        {
            for(int i = 0; i <GameManager.player.inventory.items.Length; i++)
            {
                GameObject SpawnedSlot = Instantiate(_slot, _slotParent.transform);
                _slotDatas.Add(SpawnedSlot.GetComponent<InventorySlotUI>());
                _slotDatas[i].id = i;
                SpawnedSlot.SetActive(true);
            }
            ResetSlot();
        }
        public void ResetSlot()
        {
            for(int i = 0; i < _slotDatas.Count; i++)
                _slotDatas[i].slot = GameManager.player.inventory.items[i];
            UpdateSlot();
        }
        public void UpdateSlot()
        {
            for (int i = 0; i < GameManager.player.inventory.items.Length; i++)
            {
                _slotDatas[i].SetItem(GameManager.player.inventory.items[i]);
            }
        }
        public void DeleteSlot()
        {
            if (GameManager.player.inventory.items.Length >= 0)
            {
                for (int i = 0; i < _slotDatas.Count; i++)
                    _slotDatas[i].ResetItem();
            }
        }
    }
}
