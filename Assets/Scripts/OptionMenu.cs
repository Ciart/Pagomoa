using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.UI.Title;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class OptionMenu : MonoBehaviour
    {
        public Button confirmButton;
        public Button cancleButton;
        
        [Header("Volume Slider")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _playerSlider;
        [SerializeField] private Slider _monsterSlider;
        [SerializeField] private Slider _uiSlider;
        
        private void Awake()
        {
            MatchUIFunction();
            confirmButton.onClick.AddListener(ConfirmOption);
            cancleButton.onClick.AddListener(CancelOption);
            
            Game.Instance.Sound.controller.InitAudioMixer();
            OptionData data = Game.Instance.Sound.controller.GetOptionData();
            _masterSlider.value = data.masterMixerValue;
            _musicSlider.value = data.musicMixerValue;
            _sfxSlider.value = data.sfxMixerValue;
            _playerSlider.value = data.playerMixerValue;
            _monsterSlider.value = data.monsterMixerValue;
            _uiSlider.value = data.uiMixerValue;
        }
        
        public void CancelOption()
        {
            OptionData prevData = Game.Instance.Sound.controller.GetOptionData();
            _masterSlider.value = prevData.masterMixerValue;
            _musicSlider.value = prevData.musicMixerValue;
            _sfxSlider.value = prevData.sfxMixerValue;
            _playerSlider.value = prevData.playerMixerValue;
            _monsterSlider.value = prevData.monsterMixerValue;
            _uiSlider.value = prevData.uiMixerValue;
        }
        
        public void ConfirmOption()
        {
            OptionData changerData = new OptionData()
            {
                masterMixerValue = _masterSlider.value,
                musicMixerValue = _musicSlider.value,
                sfxMixerValue = _sfxSlider.value,
                playerMixerValue = _playerSlider.value,
                monsterMixerValue = _monsterSlider.value,
                uiMixerValue = _uiSlider.value,
            };
            Game.Instance.Sound.controller.SetOptionData(changerData);
        }
        
        public void UIToggle() { gameObject.SetActive(!gameObject.activeSelf); }

        private void MatchUIFunction()
        {
            var controller = Game.Instance.Sound.controller;
            _masterSlider.onValueChanged.AddListener(controller.SetMasterVolume);
            _musicSlider.onValueChanged.AddListener(controller.SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(controller.SetSfxVolume);
            _playerSlider.onValueChanged.AddListener(controller.SetPlayerVolume);
            _monsterSlider.onValueChanged.AddListener(controller.SetMonsterVolume);
            _uiSlider.onValueChanged.AddListener(controller.SetUIVolume);
        }
    }
}
