using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.UI.Title;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class OptionMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        public OptionData optionData; 
        public Button _confirmButton;
        public Button _cancleButton;
        
        [Header("Volume Slider")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _playerSlider;
        [SerializeField] private Slider _monsterSlider;
        [SerializeField] private Slider _uiSlider;
        
        private void Awake()
        {
            AudioMixerController controller = AudioMixerController.Instance;
            Debug.Log(controller.gameObject);
            controller.masterSlider = _masterSlider;
            controller.musicSlider = _musicSlider;
            controller.sfxSlider = _sfxSlider;
            controller.playerSlider = _playerSlider;
            controller.monsterSlider = _monsterSlider;
            controller.uiSlider = _uiSlider;
            
            optionData.masterMixerValue = controller.masterSlider.value;
            optionData.musicMixerValue = controller.musicSlider.value;
            optionData.sfxMixerValue = controller.sfxSlider.value;
            optionData.teamMixerValue = controller.playerSlider.value;
            optionData.monsterMixerValue = controller.monsterSlider.value;
            optionData.uiMixerValue = controller.uiSlider.value;
            
            MakeDropDownMenu();
            controller.AddSliderFunction();
            
            dropdown.onValueChanged.AddListener(delegate{SetDropDown(dropdown.value + 1);});
            _confirmButton.onClick.AddListener(ConfirmOption);
            _cancleButton.onClick.AddListener(CancelOption);
        }
        
        private void MakeDropDownMenu()
        {
            dropdown.ClearOptions();
            for (var i = 0; i < 8; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData($"{i + 1}"));
            }
        }
        
        private void SetDropDown(int i)
        {
            //uiCanvas.scaleFactor = i;
        }
        
        public void CancelOption()
        {
            AudioMixerController controller = AudioMixerController.instance; 
            
            controller.masterSlider.value = optionData.masterMixerValue;
            controller.musicSlider.value = optionData.musicMixerValue;
            controller.sfxSlider.value = optionData.sfxMixerValue;
            controller.playerSlider.value = optionData.teamMixerValue;
            controller.monsterSlider.value = optionData.monsterMixerValue;
            controller.uiSlider.value = optionData.uiMixerValue;
            dropdown.value = (int)optionData.uiScale - 1;
        }
        
        public void ConfirmOption()
        {
            AudioMixerController controller = AudioMixerController.instance; 
            
            optionData.masterMixerValue = controller.masterSlider.value;
            optionData.musicMixerValue = controller.musicSlider.value;
            optionData.sfxMixerValue = controller.sfxSlider.value;
            optionData.teamMixerValue = controller.playerSlider.value;
            optionData.monsterMixerValue = controller.monsterSlider.value;
            optionData.uiMixerValue = controller.uiSlider.value;
        }
        
        public void UIToggle()
        {
            Debug.Log("눌림");
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
