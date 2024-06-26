using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public Slot choiceSlot;
        [SerializeField] public Slot hoverSlot;
        [SerializeField] private GameObject _slotParent;
        [SerializeField] private GameObject _slot;
        [SerializeField] private Sprite _image;
        public List<Slot> slotDatas = new List<Slot>();

        public static Inventory Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            MakeSlot();
            QuickSlotItemDB.instance.transform.SetAsLastSibling();
        }
        private void OnEnable()
        {
            DeleteSlot();
            ResetSlot();
        }
        public void MakeSlot() // slotdatas 갯수만큼 슬롯 만들기
        {
            for (int i = 0; i < InventoryDB.Instance.items.Count; i++)
            {
                GameObject spawnedslot = Instantiate(_slot, _slotParent.transform);
                slotDatas.Add(spawnedslot.GetComponent<Slot>());
                slotDatas[i].id = i;
                spawnedslot.SetActive(true);
            }
            ResetSlot();
        }
        public void ResetSlot() // 각각 슬롯에 item 할당
        {
            int i = 0;
            for (; i < slotDatas.Count; i++)
            {
                slotDatas[i].inventoryItem = InventoryDB.Instance.items[i];
            }
            UpdateSlot();
        }
        public void UpdateSlot() // List안의 Item 전체 인벤토리에 출력
        {
            for (int i = 0; i < InventoryDB.Instance.items.Count; i++)
            {
                string convert = InventoryDB.Instance.items[i].count.ToString();
                if (InventoryDB.Instance.items[i].count == 0)
                    convert = "";
            
                if (InventoryDB.Instance.items[i].item == null)
                    slotDatas[i].SetUI(_image, convert);
                else
                    slotDatas[i].SetUI(InventoryDB.Instance.items[i].item.itemImage, convert);
            }
        }
        public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
        {
            if (InventoryDB.Instance.items.Count >= 0)
            {
                for (int i = 0; i < slotDatas.Count; i++)
                    slotDatas[i].SetUI(_image, "");
            }
        }
    }
}