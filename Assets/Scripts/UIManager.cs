using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Player;
using Unity.VisualScripting;
using System;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    [SerializeField] public GameObject InventoryUI;
    [SerializeField] public GameObject EscUI;
    [SerializeField] private CinemachineVirtualCamera _inventoryCamera;

    private PlayerInput playerInput;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerDigger>().DiggingEvent.AddListener(SetDigGage);
        player.GetComponent<PlayerDigger>().digEndEvent.AddListener(SetDigGagefalse);
        player.GetComponent<Status>().oxygenAlter.AddListener(UpdateOxygenBar);
        player.GetComponent<Status>().hungryAlter.AddListener(UpdateHungryBar);
        SetDigGagefalse();

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
        bool ActiveInventory = false;
        bool OffHoverEvent = false;
        if (InventoryUI.activeSelf == false)
        {
            ActiveInventory = !ActiveInventory;
            _inventoryCamera.Priority = 11;
        }
        else
        {
            HoverEvent.Instance.HoverRenderer.SetActive(OffHoverEvent);
            _inventoryCamera.Priority = 9;
        }
        InventoryUI.SetActive(ActiveInventory);
    }

    //}

}

