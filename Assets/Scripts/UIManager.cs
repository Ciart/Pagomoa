using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Player;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    [SerializeField] public GameObject InventoryUI;
    [SerializeField] public GameObject EscUI;

    private PlayerInput playerInput;

    bool ActiveInventory = false;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerDigger>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<PlayerDigger>().digEndEvent.AddListener(SetDigGagefalse);
        player.GetComponent<Status>().oxygenAlter.AddListener(UpdateOxygenBar);
        player.GetComponent<Status>().hungryAlter.AddListener(UpdateHungryBar);
        InventoryUI.SetActive(ActiveInventory);

        playerInput = player.GetComponent<PlayerInput>();

        playerInput.Actions.SetEscUI.started += context => 
        {
            SetEscUI();
        };

        playerInput.Actions.SetInventoryUI.started += context =>
        {
            SetInventoryUI();
        };
    }
    public void UpdateOxygenBar(float current_oxygen, float max_oxygen)
    {
        oxygenbar.fillAmount = current_oxygen / max_oxygen;
    }

    public void UpdateHungryBar(float current_hungry, float max_hungry)
    {
        hungrybar.fillAmount = current_hungry / max_hungry;
    }
    public void SetDigGagefalse()
    {
        digbar.transform.parent.gameObject.SetActive(false);
    }
    public void SetDigGage(float holdtime, float digtime)
    {
        digbar.fillAmount = holdtime / digtime;
        digbar.transform.parent.gameObject.SetActive(true);
    }
    private void SetEscUI()
    {
        bool activeEscUI = false;
        if (EscUI.activeSelf == false)
            activeEscUI = !activeEscUI;
        EscUI.SetActive(activeEscUI);
    }
    private void SetInventoryUI()
    {
        ActiveInventory = !ActiveInventory;
        InventoryUI.SetActive(ActiveInventory);
        if (InventoryUI.activeSelf == false)
            HoverEvent.Instance.image.SetActive(ActiveInventory);
    }

    //}

}

