using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        public Image oxygenbar;
        public Image hungrybar;
        public GameObject inventoryUIPrefab;
        public GameObject escUI;
        public GameObject interactableUI;
        public CinemachineVirtualCamera inventoryCamera;

        private PlayerInput _playerInput;
        private GameObject _inventoryUI;
        private bool _isActiveInventory;

        private void Start()
        {
            base.Awake();
            
            _inventoryUI = Instantiate(inventoryUIPrefab, transform);
            _inventoryUI.SetActive(false);   
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;

            player.GetComponent<PlayerStatus>().oxygenAlter.AddListener(UpdateOxygenBar);
            player.GetComponent<PlayerStatus>().hungryAlter.AddListener(UpdateHungryBar);

            _playerInput = player.GetComponent<PlayerInput>();

            _playerInput.Actions.Menu.performed += context => { ToggleEscUI(); };

            _playerInput.Actions.Inventory.performed += context => { ToggleInventoryUI(); };
        }

        public void UpdateOxygenBar(float current_oxygen, float max_oxygen)
        {
            oxygenbar.fillAmount = current_oxygen / max_oxygen;
        }

        public void UpdateHungryBar(float current_hungry, float max_hungry)
        {
            hungrybar.fillAmount = current_hungry / max_hungry;
        }

        private void ToggleEscUI()
        {
            escUI.SetActive(!escUI.activeSelf);
        }

        private void ToggleInventoryUI()
        {
            _isActiveInventory = !_isActiveInventory;
            _inventoryUI.SetActive(_isActiveInventory);

            if (_isActiveInventory)
            {
                inventoryCamera.Priority = 11;
            }
            else
            {
                inventoryCamera.Priority = 9;

                if (InventoryUIManager.Instance.ItemHoverObject.activeSelf == true)
                    InventoryUIManager.Instance.ItemHoverObject.SetActive(false);

                    if (Inventory.Inventory.Instance.hoverSlot == null)
                {
                    return;
                }

                Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().boostImage.sprite =
                    Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().hoverImage[1];
            }
        }

        public static GameObject CreateInteractableUI(Transform parent)
        {
            return Instantiate(instance.interactableUI, parent);
        }
    }
}
