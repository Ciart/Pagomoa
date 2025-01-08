using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Systems
{
    public class UIManager : PManager<UIManager>
    {
        ~UIManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private UIContainer _uiContainer;
        private PlayerInput _playerInput;
        private GameObject _inventoryUI;
        private GameObject _dialogueUI;
        private GameObject _quickSlot;
        private DaySummaryUI _daySummaryUI;
        private bool _isActiveInventory;
        private FadeUI _fadeUI;
        public UIContainer GetUIContainer() { return _uiContainer; }
        
        public override void Awake()
        {
            _uiContainer = DataBase.data.GetUIData();
            
            _inventoryUI = Object.Instantiate(_uiContainer.inventoryUIPrefab, _uiContainer.transform);
            _inventoryUI.SetActive(false);
            
            _dialogueUI = Object.Instantiate(_uiContainer.dialogueUIPrefab, _uiContainer.transform);
            _dialogueUI.SetActive(false);
            _uiContainer.dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            _daySummaryUI = Object.Instantiate(_uiContainer.daySummaryUIPrefab, _uiContainer.transform);
            _daySummaryUI.gameObject.SetActive(false);

            _fadeUI = _uiContainer.fadeUI.GetComponent<FadeUI>();
            
            _quickSlot = Object.Instantiate(_uiContainer.quickSlotContainerUIPrefab, _uiContainer.transform);
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

        public void ShowDaySummaryUI()
        {
            _daySummaryUI.gameObject.SetActive(true);
        }

        public void DeActiveUI()
        {
            _uiContainer.miniMap.SetActive(false);
            _uiContainer.statePanel.SetActive(false);
            _uiContainer.oxygenBar.gameObject.SetActive(false);
            _quickSlot.SetActive(false);
        }

        public void ActiveUI()
        {
            _uiContainer.miniMap.SetActive(true);
            _uiContainer.statePanel.SetActive(true);
            _uiContainer.oxygenBar.gameObject.SetActive(true);
            _quickSlot.SetActive(true);
        }
        
        private void ToggleEscDialogueUI()
        {
            _dialogueUI.SetActive(false);
        }

        public void PlayFadeAnimation(FadeFlag flag, float duration)
        {
            _fadeUI.gameObject.SetActive(true);
            _fadeUI.Fade(flag, duration);
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
