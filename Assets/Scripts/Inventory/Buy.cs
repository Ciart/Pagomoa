using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy : MonoBehaviour
{
    [SerializeField] private AuctionDB auctionDB;
    [SerializeField] public Slot choiceSlot;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject slot;
    [SerializeField] public  GameObject gold;
    [SerializeField] private List<Slot> slots = new List<Slot>();


    private static Buy instance;
    public static Buy Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(Buy)) as Buy;
            }
            return instance;
        }
    }
    private void Awake()
    {
        AuctionSlot();
    }
    public void AuctionSlot()
    {
        for(int i = 0; i < auctionDB.auctionItem.Count; i++) 
        {
            GameObject SpawnedSlot = Instantiate(slot, slotParent.transform);
            slots.Add(SpawnedSlot.GetComponent<Slot>());
            SpawnedSlot.SetActive(true);
        }
        ResetSlot();
    }
    public void ResetSlot() 
    {
        int i = 0;
        for (; i < auctionDB.auctionItem.Count; i++)
            slots[i].inventoryItem = auctionDB.auctionItem[i];
        UpdateSlot();
    }
    public void UpdateSlot() 
    {
        for (int i = 0; i < auctionDB.auctionItem.Count; i++)
        {
            string convert = auctionDB.auctionItem[i].count.ToString();
            if (auctionDB.auctionItem[i].count == 0)
                convert = "";
            slots[i].SetUI(auctionDB.auctionItem[i].item.itemImage, convert);
        }
    }
    public void DestroySlot()
    {
        for (int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);
        slots.Clear();
    }
}
