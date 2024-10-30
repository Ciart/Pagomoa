using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI.Book;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Systems
{
    public class UIManager : PManager
    {
        public static UIManager instance { get; private set; } 
        public UIManager() { instance ??= this; }

        ~UIManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private UIContainer _uiContainer;
        private PlayerInput _playerInput;
        private GameObject _inventoryUI;
        private GameObject _dialogueUI;
        private bool _isActiveInventory;

        public UIContainer GetUIContainer() { return _uiContainer; }
        
        public override void Awake()
        {
            _uiContainer = DataBase.data.GetUIData();
            
            _inventoryUI = Object.Instantiate(_uiContainer.inventoryUIPrefab, _uiContainer.transform);
            Debug.Log(_inventoryUI);
            _inventoryUI.SetActive(false);
            
            _dialogueUI = Object.Instantiate(_uiContainer.dialogueUIPrefab, _uiContainer.transform);
            _dialogueUI.SetActive(false);
            _uiContainer.dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            Object.Instantiate(_uiContainer.quickSlotContainerUIPrefab, _uiContainer.transform);
        }

        public override void Start()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;

            player.GetComponent<PlayerStatus>().oxygenAlter.AddListener(UpdateOxygenBar);
            player.GetComponent<PlayerStatus>().hungryAlter.AddListener(UpdateHungryBar);

            _playerInput = player.GetComponent<PlayerInput>();

            _playerInput.Actions.Menu.performed += context => { ToggleEscUI(); };

            _playerInput.Actions.Menu.performed += context => { ToggleEscDialogueUI(); };

            _playerInput.Actions.Inventory.performed += context => { ToggleInventoryUI(); };
        }

        private void UpdateOxygenBar(float currentOxygen, float maxOxygen)
        {
            _uiContainer.oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }

        private void UpdateHungryBar(float currentHungry, float maxHungry)
        {
            _uiContainer.hungryBar.fillAmount = currentHungry / maxHungry;
        }

        private void ToggleEscUI()
        {
            if(!_dialogueUI.activeSelf)
                _uiContainer.escUI.SetActive(!_uiContainer.escUI.activeSelf);
        }

        private void ToggleInventoryUI()
        {
            _isActiveInventory = !_isActiveInventory;
            _inventoryUI.SetActive(_isActiveInventory);

            // if (_isActiveInventory)
            // {
            //     inventoryCamera.Priority = 11;
            // }
            // else
            // {
            //     inventoryCamera.Priority = 9;
            //
            //     // if (InventoryUIManager.Instance.ItemHoverObject.activeSelf == true)
            //     //     InventoryUIManager.Instance.ItemHoverObject.SetActive(false);
            //
            //     if (Inventory.Inventory.Instance.hoverSlot == null)
            //         return;
            //
            //     Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().boostImage.sprite =
            //         Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().hoverImage[1];
            // }
        }

        private void ToggleEscDialogueUI()
        {
            _dialogueUI.SetActive(false);
        }

        public GameObject CreateInteractableUI(Transform parent)
        {
            return Object.Instantiate(DataBase.data.GetUIData().interactableUI, parent);
        }

        public GameObject CreateQuestCompleteUI(Transform parent)
        {
            return Object.Instantiate(DataBase.data.GetUIData().questCompleteUI, parent);
        }

    }
}
