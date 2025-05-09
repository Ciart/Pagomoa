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
        private UIContainer _ui;
        ~UIManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        private PlayerInput _playerInput;
        private GameObject _dialogueUI;
        private DaySummaryUI _daySummaryUI;
        
        public TitleUI titleUI { get; private set; }
        public MinimapUI minimapUI { get; private set; }
        public StatusUI StatusUI { get; private set; }
        public BookUI bookUI {get; private set;}
        public ShopUI shopUI {get; private set;}
        public QuickUI quickUI {get; private set;}
        public FadeUI fadeUI {get; private set;}
        public EscOptionUI escOptionUI {get; private set;}
        
        public UIContainer GetUIContainer() { return _ui; }
        
        private bool _isActiveInventory;
        
        public override void PreStart()
        {
            _ui = Object.Instantiate(DataBase.data.GetUIData());
            titleUI = Object.Instantiate(_ui.title, _ui.transform);
            
            minimapUI = Object.Instantiate(_ui.miniMapPanelPrefab, _ui.transform);
            StatusUI = Object.Instantiate(_ui.statusPanelPrefab, _ui.transform);
            
            bookUI = Object.Instantiate(_ui.bookUIPrefab, _ui.transform);
            bookUI.gameObject.SetActive(_isActiveInventory);
            
            shopUI = Object.Instantiate(_ui.shopUIPrefab, _ui.transform);
            quickUI = Object.Instantiate(_ui.quickUIPrefab, _ui.transform);
            
            _dialogueUI = Object.Instantiate(_ui.dialogueUIPrefab, _ui.transform);
            _dialogueUI.SetActive(false);
            _ui.dialogueUI = _dialogueUI.GetComponent<DialogueUI>();

            _daySummaryUI = Object.Instantiate(_ui.daySummaryUIPrefab, _ui.transform);
            _daySummaryUI.gameObject.SetActive(false);

            fadeUI = Object.Instantiate(_ui.fadeUIPrefab, _ui.transform);
            escOptionUI = Object.Instantiate(_ui.escUI, _ui.transform);
        }

        public override void Start()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        public override void FixedUpdate()
        {
            if (minimapUI)
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
            var playerGold = Game.Instance.player.inventory.gold;

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
            if (!minimapUI) return;
            minimapUI.gameObject.SetActive(false);
            StatusUI.gameObject.SetActive(false);
            quickUI.gameObject.SetActive(false);
            
            bookUI.DeActiveBook();
        }

        public void ActiveUI()
        {
            if (!minimapUI) return;
            minimapUI.gameObject.SetActive(true);
            StatusUI.gameObject.SetActive(true);
            quickUI.gameObject.SetActive(true);
        }

        public void PlayFadeAnimation(FadeFlag flag, float duration)
        {
            if (fadeUI.gameObject.activeSelf) return;
            fadeUI.gameObject.SetActive(true);
            fadeUI.Fade(flag, duration);
        }
        
        public GameObject CreateInteractableUI(Transform parent)
        {
            return Object.Instantiate(DataBase.data.GetUIData().interactableUI, parent);
        }

        public QuestCompleteIcon CreateQuestCompleteUI(Transform parent)
        {
            var uiObject = Object.Instantiate(DataBase.data.GetUIData().questCompleteUI, parent);
            return uiObject.GetComponent<QuestCompleteIcon>();
        }
    }
}
