using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class InfoUI : UIBehaviour
    {
        [Header("Info Tabs")]
        [SerializeField] private Button _upgradeTabButton;
        [SerializeField] private Button _powerStoneTabButton;
        [SerializeField] private Image _upgradeTabImage;
        [SerializeField] private Image _powerStoneTabImage;
        
        [Header("Upgrade Tab")] 
        [SerializeField] private GameObject _upgradeLeftPage;
        [SerializeField] private GameObject _upgradeRightPage;
        
        [SerializeField] private TextMeshProUGUI[] _needStoneText;
        [SerializeField] private Image[] _needStoneImage;

        [SerializeField] private TextMeshProUGUI _drillName;
        [SerializeField] private TextMeshProUGUI _drillDescription;

        [SerializeField] private Button _upgradeBtn;
        
        [Header("PowerStone Tab")]
        [SerializeField] private GameObject _powerStoneLeftPage;
        [SerializeField] private PowerStoneInfoUI _powerStoneRightPage;
        
        
        private void SetMineralCount(string id, int count)
        {
            UIUpdate();
        }

        protected override void Start()
        {
            base.Start();
            
            _upgradeBtn?.onClick.AddListener(Game.Instance.player.drill.DrillUpgrade);
            _upgradeBtn?.onClick.AddListener(UIUpdate);

            _upgradeTabButton?.onClick.AddListener(ToggleInfoTab);
            _powerStoneTabButton?.onClick.AddListener(ToggleInfoTab);
        }

        private void UIUpdate()
        {
            if (!Game.Instance.player) return;

            var nowDrill = Game.Instance.player.drill.nowDrill;
            _drillName.text = nowDrill.drillName;
            _drillDescription.text = nowDrill.drillDescription;
            
            var nextDrill = Game.Instance.player.drill.nextDrill;

            for (int i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var mineralID = nextDrill.upgradeNeeds[i].mineral.id;
                var mineralName = nextDrill.upgradeNeeds[i].mineral.name;
                var currentCount = MineralCollector.GetMineralCount(mineralID);
                var maxCount = nextDrill.upgradeNeeds[i].count;

                _needStoneImage[i].sprite = Resources.Load<Sprite>("Items/" + mineralID);
                _needStoneImage[i].enabled = true;
                _needStoneText[i].text = $"{mineralName} {currentCount}/{maxCount}";
                _needStoneText[i].enabled = true;
            }
            for (int i = nextDrill.upgradeNeeds.Length; i < _needStoneText.Length; i++)
            {
                _needStoneText[i].enabled = false;
                _needStoneImage[i].enabled = false;
            }
        }

        private void ToggleInfoTab()
        {
            _upgradeLeftPage.SetActive(!_upgradeLeftPage.activeSelf);
            _upgradeRightPage.SetActive(!_upgradeRightPage.activeSelf);
            _powerStoneLeftPage.SetActive(!_powerStoneLeftPage.activeSelf);
            _powerStoneRightPage.gameObject.SetActive(!_powerStoneRightPage.gameObject.activeSelf);
            _upgradeTabImage.gameObject.SetActive(!_upgradeTabImage.gameObject.activeSelf);
            _powerStoneTabImage.gameObject.SetActive(!_powerStoneTabImage.gameObject.activeSelf);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            UIUpdate();
            MineralCollector.OnMineralsChange += SetMineralCount;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MineralCollector.OnMineralsChange -= SetMineralCount;
            _upgradeLeftPage.SetActive(true);
            _upgradeRightPage.SetActive(true);
            _upgradeTabImage.gameObject.SetActive(true);
            _powerStoneLeftPage.SetActive(false);
            _powerStoneRightPage.gameObject.SetActive(false);
            _powerStoneTabImage.gameObject.SetActive(false);
        }
    }
}
