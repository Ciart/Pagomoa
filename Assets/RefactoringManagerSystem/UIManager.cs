using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa.Systems
{
    public class UIManager : Manager<UIManager>
    {
        ~UIManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private UIContainer _uiContainer;
        private PlayerInput _playerInput;
        private GameObject _dialogueUI;
        private DaySummaryUI _daySummaryUI;
        
        public MinimapUI minimapUI { get; private set; }
        public StateUI stateUI { get; private set; }
        public BookUI bookUI {get; private set;}
        public ShopUI shopUI {get; private set;}
        public QuickUI quickUI {get; private set;}
        public FadeUI fadeUI {get; private set;}
        
        public UIContainer GetUIContainer() { return _uiContainer; }
        
        private bool _isActiveInventory;
        
        public override void PreStart()
        {
            _uiContainer = DataBase.data.GetUIData();

            minimapUI = Object.Instantiate(_uiContainer.miniMapPanelPrefab, _uiContainer.transform);
            stateUI = Object.Instantiate(_uiContainer.statePanelPrefab, _uiContainer.transform);
            
            bookUI = Object.Instantiate(_uiContainer.bookUIPrefab, _uiContainer.transform);
            bookUI.gameObject.SetActive(_isActiveInventory);
            
            shopUI = Object.Instantiate(_uiContainer.shopUIPrefab, _uiContainer.transform);
            quickUI = Object.Instantiate(_uiContainer.quickUIPrefab, _uiContainer.transform);
            
            _dialogueUI = Object.Instantiate(_uiContainer.dialogueUIPrefab, _uiContainer.transform);
            _dialogueUI.SetActive(false);
            _uiContainer.dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            _daySummaryUI = Object.Instantiate(_uiContainer.daySummaryUIPrefab, _uiContainer.transform);
            _daySummaryUI.gameObject.SetActive(false);

            fadeUI = Object.Instantiate(_uiContainer.fadeUIPrefab, _uiContainer.transform);
        }

        public override void Start()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        public override void FixedUpdate()
        {
            minimapUI.UpdateMinimap();
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;
            
            player.GetComponent<PlayerStatus>().oxygenAlter.AddListener(stateUI.UpdateOxygenBar);
            //player.GetComponent<PlayerStatus>().hungryAlter.AddListener(UpdateHungryBar);

            _playerInput = player.GetComponent<PlayerInput>();
            _playerInput.Actions.Menu.performed += context => { ToggleEscUI(); };
            _playerInput.Actions.Menu.performed += context => { ToggleEscDialogueUI(); };
            _playerInput.Actions.Inventory.performed += context => { ToggleInventoryUI(); };
            
            UpdateGoldUI();
        }

        public void UpdateGoldUI()
        {
            var playerGold = Game.instance.player.inventory.gold;

            shopUI.shopGoldUI.text = playerGold.ToString();
            stateUI.playerGoldUI.text = playerGold.ToString();        
        }
        
        private void ToggleEscUI()
        {
            bookUI.DeActiveBook();
            shopUI.DeActiveShop();
            _dialogueUI.SetActive(false);
        }

        private void ToggleInventoryUI()
        {
            if (Game.Instance.UI.shopUI.gameObject.activeSelf) return;
            if (_dialogueUI.gameObject.activeSelf)
            {
                bookUI.DeActiveBook();
                return;
            }
            
            _isActiveInventory = !_isActiveInventory;
            bookUI.gameObject.SetActive(_isActiveInventory);
            
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
            minimapUI.gameObject.SetActive(false);
            stateUI.gameObject.SetActive(false);
            quickUI.gameObject.SetActive(false);
        }

        public void ActiveUI()
        {
            minimapUI.gameObject.SetActive(true);
            stateUI.gameObject.SetActive(true);
            quickUI.gameObject.SetActive(true);
        }
        
        private void ToggleEscDialogueUI()
        {
            _dialogueUI.SetActive(false);
        }

        public void PlayFadeAnimation(FadeFlag flag, float duration)
        {
            fadeUI.gameObject.SetActive(true);
            fadeUI.Fade(flag, duration);
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
