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
        public GameObject dialogueUIPrefab;
        public GameObject escUI;
        public GameObject interactableUI;
        public GameObject questCompleteUI;
        public QuestUI questUI;
        public DialogueUI dialogueUI;
        public CinemachineVirtualCamera inventoryCamera;

        private PlayerInput _playerInput;
        private GameObject _inventoryUI;
        private GameObject _dialogueUI;
        private bool _isActiveInventory;

        protected override void Awake()
        {
            base.Awake();
            
            _inventoryUI = Instantiate(inventoryUIPrefab, transform);
            _inventoryUI.SetActive(false);
            questUI = _inventoryUI.GetComponentInChildren<QuestUI>();

            _dialogueUI = Instantiate(dialogueUIPrefab, transform);
            _dialogueUI.SetActive(false);
            dialogueUI = _dialogueUI.GetComponent<DialogueUI>();
        }
        
        private void Start()
        {
            // EventManager.AddListener<AddNpcImageEvent>(questUI.AddNpcImages);
            // EventManager.AddListener<MakeQuestListEvent>(questUI.MakeQuestList);
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

            _playerInput.Actions.Menu.performed += context => { ToggleEscDialogueUI(); };

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
            if(!_dialogueUI.activeSelf)
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
                    return;

                Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().boostImage.sprite =
                    Inventory.Inventory.Instance.hoverSlot.GetComponent<Hover>().hoverImage[1];
            }
        }
        private void ToggleEscDialogueUI()
        {
            _dialogueUI.SetActive(false);
        }

        public static GameObject CreateInteractableUI(Transform parent)
        {
            return Instantiate(instance.interactableUI, parent);
        }

        public static GameObject CreateQuestCompleteUI(Transform parent)
        {
            return Instantiate(instance.questCompleteUI, parent);
        }

    }
}
