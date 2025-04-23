using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger;
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
        public StatusUI StatusUI { get; private set; }
        public BookUI bookUI {get; private set;}
        public ShopUI shopUI {get; private set;}
        public QuickUI quickUI {get; private set;}
        public FadeUI fadeUI {get; private set;}
        public EscOptionUI escOptionUI {get; private set;}
        
        public UIContainer GetUIContainer() { return _uiContainer; }
        
        private bool _isActiveInventory;
        
        public override void PreStart()
        {
            _uiContainer = DataBase.data.GetUIData();

            minimapUI = Object.Instantiate(_uiContainer.miniMapPanelPrefab, _uiContainer.transform);
            StatusUI = Object.Instantiate(_uiContainer.statusPanelPrefab, _uiContainer.transform);
            
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
            escOptionUI = Object.Instantiate(_uiContainer.escUI, _uiContainer.transform);
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

            _playerInput = player.GetComponent<PlayerInput>();
            _playerInput.Actions.Menu.performed += context => { ToggleEscUI(); };
            _playerInput.Actions.Inventory.performed += context => { ToggleInventoryUI(); };
            
            UpdateGoldUI();
        }

        public void UpdateGoldUI()
        {
            var playerGold = Game.instance.player.inventory.gold;

            shopUI.shopGoldUI.text = playerGold.ToString();
            StatusUI.playerGoldUI.text = playerGold.ToString();        
        }
        
        private void ToggleEscUI()
        {
            // 1. 일일 요약 창이 켜져 있으면 ESC 입력 무시
            if (_daySummaryUI.gameObject.activeSelf)
                return;

            bool wasAnyUIClosed = false;

            if (bookUI.gameObject.activeSelf)
            {
                bookUI.DeActiveBook();
                _isActiveInventory = false;
                wasAnyUIClosed = true;
            }

            if (shopUI.gameObject.activeSelf)
            {
                shopUI.DeActiveShop();
                Game.Instance.Time.ResumeTime();
                Game.Instance.Time.ResumeTime();
                wasAnyUIClosed = true;
            }
            
            if (_dialogueUI.activeSelf)
            {
                _dialogueUI.SetActive(false);
                Game.Instance.Time.ResumeTime();
                Game.Instance.Dialogue.StopStory();
                wasAnyUIClosed = true;
            }

            if (!wasAnyUIClosed)
            {
                bool isActive = escOptionUI.gameObject.activeSelf;
                escOptionUI.gameObject.SetActive(!isActive);

                if (!isActive)
                    Game.Instance.Time.PauseTime();
                else
                    Game.Instance.Time.ResumeTime();
            }
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
        }

        public void ShowDaySummaryUI()
        {
            _daySummaryUI.gameObject.SetActive(true);
        }

        public void DeActiveUI()
        {
            minimapUI.gameObject.SetActive(false);
            StatusUI.gameObject.SetActive(false);
            quickUI.gameObject.SetActive(false);
            
            bookUI.DeActiveBook();
        }

        public void ActiveUI()
        {
            minimapUI.gameObject.SetActive(true);
            StatusUI.gameObject.SetActive(true);
            quickUI.gameObject.SetActive(true);
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

        public QuestCompleteIcon CreateQuestCompleteUI(Transform parent)
        {
            return Object.Instantiate(DataBase.data.GetUIData().questCompleteUI, parent);
        }

    }
}
