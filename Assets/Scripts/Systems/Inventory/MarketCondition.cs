using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarketCondition : MonoBehaviour
{
    [SerializeField] private AchieveContents achieveContents;
    [SerializeField] private GameObject slotContent;
    [SerializeField] private GameObject slot;
    [SerializeField] private int count = 0;
    public List<AchieveContents> contentDatas = new List<AchieveContents>();

    public static MarketCondition Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void MakeSlot()
    {
        for (; count < Achievements.Instance.AchieveMinerals.Count; count++)
        {
            GameObject SpawnedSlot = Instantiate(slot, slotContent.transform);
            contentDatas.Add(SpawnedSlot.GetComponent<AchieveContents>());
            SpawnedSlot.SetActive(true);
        }
        UpdateSlot();
    }
    private void UpdateSlot()
    {
        for (int i = 0; i < contentDatas.Count; i++)
        {
            contentDatas[i].SetUI(Achievements.Instance.AchieveMinerals[i].item.itemImage, 
                Achievements.Instance.AchieveMinerals[i].item.itemPrice);
        }
    }
}
