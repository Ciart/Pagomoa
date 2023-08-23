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
    public void MakeSlot() // slotDatas ������ŭ ���� �����
    {
        for (int i = 0; i < count; i++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            slotDatas.Add(SpawnedSlot.GetComponent<Slot>());
            slotDatas[i].id = i;
            SpawnedSlot.SetActive(true);
        }
    }
    public void ResetSlot() // ���� ���Կ� item �Ҵ�
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
    public void UpdateSlot() // List���� Item ��ü �κ��丮�� ���
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
    public void DeleteSlot() // �κ��丮�� ��µ� �����۵� ���� NULL
    {
        if (inventoryDB.items.Count >= 0 )
        {
            for (int i = 0; i < slotDatas.Count; i++)
                slotDatas[i].SetUI(image, "");
        }
    }
}