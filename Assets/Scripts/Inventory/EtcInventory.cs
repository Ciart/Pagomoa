using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EtcInventory : MonoBehaviour
{
    [SerializeField] private InventoryDB inventoryDB;
    [SerializeField] public Slot choiceSlot;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject slot;
    [SerializeField] public GameObject gold;
    [SerializeField] private Sprite image;
    public  List<Slot> slotDatas = new List<Slot>();
    [SerializeField] private int count;

    private static EtcInventory instance;
    public static EtcInventory Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(EtcInventory)) as EtcInventory;
            }
            return instance;
        }
    }
    private void Awake()
    {
        UpdateSlot();
    }
    public void MakeSlot() // slotDatas 갯수만큼 슬롯 만들기
    {
        for (int i = 0; i < count; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            slotDatas.Add(SpawnedSlot.GetComponent<Slot>());
            slotDatas[i].id = i;
            SpawnedSlot.SetActive(true);
        }
    }
    public void ResetSlot() // 각각 슬롯에 item 할당
    {
        int i = 0;
        for(; i < inventoryDB.items.Count && i < slotDatas.Count; i++)
        {
            slotDatas[i].inventoryItem = inventoryDB.items[i];
        }
        for(; i< slotDatas.Count; i++)
        {
            slotDatas[i].inventoryItem = null;
        }
        UpdateSlot();
    }
    public void UpdateSlot() // List안의 Item 전체 인벤토리에 출력
    {
        DeleteSlot();
        for (int i = 0; i < inventoryDB.items.Count; i++)
        {
            string convert = inventoryDB.items[i].count.ToString();
            if (inventoryDB.items[i].count == 0)
            {
                convert = "";
            }
            slotDatas[i].SetUI(inventoryDB.items[i].item.itemImage, convert);
        }
        gold.GetComponent<Text>().text = inventoryDB.Gold.ToString();
    }
    public void DeleteSlot() // 인벤토리에 출력된 아이템들 전부 NULL
    {
        if (inventoryDB.items.Count >= 0 )
        {
            for (int i = 0; i < slotDatas.Count; i++)
                slotDatas[i].SetUI(image, "");
        }
    }
}