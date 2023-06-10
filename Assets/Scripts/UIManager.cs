using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    public GameObject InventoryUI;
    public GameObject ScrollView;
    bool ActiveInventory = false;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Dig>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<PlayerController>().oxygen_alter.AddListener(UpdateOxygenBar);
        player.GetComponent<PlayerController>().hungry_alter.AddListener(UpdateHungryBar);
        player.GetComponent<PlayerController>().direction_alter.AddListener(SetPlayerUIDirection);
        InventoryUI.SetActive(ActiveInventory);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory = !ActiveInventory;
            InventoryUI.SetActive(ActiveInventory);
            ScrollView.transform.Find("EquipmentViewPoint").gameObject.SetActive(true);
            ScrollView.transform.Find("ConsumptionViewPoint").gameObject.SetActive(false);
            ScrollView.transform.Find("EtcViewPoint").gameObject.SetActive(false);                 
        }
    }
    public void UpdateOxygenBar(float current_oxygen, float max_oxygen)
    {
        oxygenbar.fillAmount = current_oxygen / max_oxygen;
    }

    public void UpdateHungryBar(float current_hungry, float max_hungry)
    {
        hungrybar.fillAmount = current_hungry / max_hungry;
    }
    public void SetPlayerUIDirection(float direction)
    {
        transform.localScale = new Vector3(direction, 1, 1);
    }
    public void SetDigGagefalse()
    {
        digbar.enabled = false;
    }
    public void SetDigGage(float holdtime, float digtime)
    {
        digbar.fillAmount = holdtime / digtime;
        digbar.enabled = true;
    }
}