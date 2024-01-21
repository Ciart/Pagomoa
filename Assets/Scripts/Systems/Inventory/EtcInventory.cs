using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EtcInventory : MonoBehaviour
{
    [SerializeField] public Slot choiceSlot;
    [SerializeField] public Slot hoverSlot;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject slot;
    [SerializeField] public GameObject gold;
    [SerializeField] private Sprite image;
    public List<Slot> slotDatas = new List<Slot>();
    [SerializeField] public int count;

    public static EtcInventory Instance = null;
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
            GameObject spawnedslot = Instantiate(slot, slotParent.transform);
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
                slotDatas[i].SetUI(image, convert);
            else
                slotDatas[i].SetUI(InventoryDB.Instance.items[i].item.itemImage, convert);
        }
        gold.GetComponent<Text>().text = InventoryDB.Instance.Gold.ToString();
    }
    public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
    {
        if (InventoryDB.Instance.items.Count >= 0)
        {
            for (int i = 0; i < slotDatas.Count; i++)
                slotDatas[i].SetUI(image, "");
        }
    }
}