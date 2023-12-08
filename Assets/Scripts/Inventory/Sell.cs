using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sell : MonoBehaviour
{
    public static Sell Instance = null;

    [SerializeField] public Slot choiceSlot;
    [SerializeField] public ShopHover hovering;
    [SerializeField] private GameObject _slot;
    [SerializeField] private GameObject _slotParent;
    [SerializeField] private Sprite _image;
    [SerializeField] public TextMeshProUGUI gold;
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
    }
    public void MakeSlot()
    {
        for(int i = 0; i < InventoryDB.Instance.items.Count; i++)
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
            _slotDatas[i].inventoryItem = InventoryDB.Instance.items[i];
        UpdateSlot();
    }
    public void UpdateSlot()
    {
        for (int i = 0; i < InventoryDB.Instance.items.Count; i++)
        {
            string convert = InventoryDB.Instance.items[i].count.ToString();
            if (InventoryDB.Instance.items[i].count == 0)
            {
                convert = "";
            }
            if (InventoryDB.Instance.items[i].item == null)
                _slotDatas[i].SetUI(_image, convert);
            else
                _slotDatas[i].SetUI(InventoryDB.Instance.items[i].item.itemImage, convert);
        }
        gold.text = InventoryDB.Instance.Gold.ToString();
    }
    public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
    {
        if (InventoryDB.Instance.items.Count >= 0)
        {
            for (int i = 0; i < _slotDatas.Count; i++)
                _slotDatas[i].SetUI(_image, "");
        }
    }
}
