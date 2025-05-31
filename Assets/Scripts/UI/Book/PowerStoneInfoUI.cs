using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class PowerStoneInfoUI : UIBehaviour
    {
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI _stoneNameText;
        [SerializeField] private TextMeshProUGUI _stoneDescriptionText;
        
        [Header("PassiveTab")]
        [SerializeField] private Button _passiveTabButton;
        [SerializeField] private GameObject _passiveTab; 
        
        [Header("ActiveTab")]
        [SerializeField] private Button _activeTabButton;
        [SerializeField] private GameObject _activeTab;

        private void ToggleSkillTab()
        {
            _passiveTab.SetActive(!_passiveTab.activeSelf);
            _activeTab.SetActive(!_activeTab.activeSelf);
        }
        
        protected override void Start()
        {
            base.Start();
            _passiveTabButton?.onClick.AddListener(ToggleSkillTab);
            _activeTabButton?.onClick.AddListener(ToggleSkillTab);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            var stone = Game.Instance.player.inventory.FindSameItem("YellowStone");

            if (stone.Count > 1)
            {
                _stoneNameText.text = "옐로스톤";
                _stoneDescriptionText.text = "사막에서 발견한 파워스톤이다." +
                                             "\n처음으로 찾았어!!!!";
            }
            else
            {
                _stoneNameText.text = "???";
                _stoneDescriptionText.text = "이곳의 전부를 찾아보자!";
            }
            
            // TODO : 노란 파워스톤을 먹었을 시 추가할 여러 기능이 필요하다.
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _passiveTab.SetActive(true);
            _activeTab.SetActive(false);
        }
    }
}
