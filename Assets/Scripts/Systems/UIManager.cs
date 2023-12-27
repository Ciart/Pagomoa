using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;
using Cinemachine;
using Entities;
using Entities.Players;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    [SerializeField] Image digbar;
    [SerializeField] public GameObject InventoryUI;
    [SerializeField] public GameObject EscUI;
    [SerializeField] private CinemachineVirtualCamera _inventoryCamera;

    private GameObject _UI;
    private bool _activeInventory = false;
    private PlayerInput playerInput;

    private void Awake()
    {
        _UI = GameObject.Find("UI");

        EntityManager.instance.spawnedPlayer += player =>
        {
            player.GetComponent<PlayerStatus>().oxygenAlter.AddListener(UpdateOxygenBar);
            player.GetComponent<PlayerStatus>().hungryAlter.AddListener(UpdateHungryBar);

            playerInput = player.GetComponent<PlayerInput>();

            playerInput.Actions.SetEscUI.started += context => 
            {
                SetEscUI();
            };

            playerInput.Actions.SetInventoryUI.started += context =>
            {
                CreateInventoryUI();
            };
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
    private void SetEscUI()
    {
        bool activeEscUI = false;
        if (EscUI.activeSelf == false)
            activeEscUI = !activeEscUI;
        EscUI.SetActive(activeEscUI);
    }
    private void SetInventoryUI()
    {
        bool OffHoverEvent = false;
        if (_activeInventory == false)
        {
            _activeInventory = !_activeInventory;
            _UI.transform.Find("Inventory(Clone)").gameObject.SetActive(_activeInventory);
            _inventoryCamera.Priority = 11;
        }
        else
        {
            _activeInventory = !_activeInventory;
            HoverEvent.Instance.HoverRenderer.SetActive(OffHoverEvent);
            _UI.transform.Find("Inventory(Clone)").gameObject.SetActive(_activeInventory);
            _inventoryCamera.Priority = 9;
        }
    }
    private void CreateInventoryUI()
    {

        if (_UI.transform.Find("Inventory(Clone)") == null)
        {
            Instantiate(InventoryUI, transform).SetActive(false);
            SetInventoryUI();
        }
        else
            SetInventoryUI();
    }
    //}

}

