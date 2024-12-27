using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.UI.Title;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class Change : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown; 
        
        public CanvasScaler uiCanvas;
        
        private void Start()
        {
            OptionDB.instance.uiScale = uiCanvas.scaleFactor;          
            
            MakeDropDownMenu();
            
            dropdown.onValueChanged.AddListener(delegate{SetDropDown(dropdown.value + 1);});
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
            uiCanvas.scaleFactor = i;
        }
        
        public void CancelOption()
        {
            AudioMixerController.instance.masterSlider.value = OptionDB.instance.masterMixerValue;
            AudioMixerController.instance.musicSlider.value = OptionDB.instance.musicMixerValue;
            AudioMixerController.instance.sfxSlider.value = OptionDB.instance.sfxMixerValue;
            AudioMixerController.instance.teamSlider.value = OptionDB.instance.teamMixerValue;
            AudioMixerController.instance.monsterSlider.value = OptionDB.instance.monsterMixerValue;
            AudioMixerController.instance.uiSlider.value = OptionDB.instance.uiMixerValue;
            dropdown.value = (int)OptionDB.instance.uiScale - 1;
            uiCanvas.scaleFactor = OptionDB.instance.uiScale;
            
            gameObject.SetActive(false);
        }
        
        public void ConfirmOption()
        {
            OptionDB.instance.masterMixerValue = AudioMixerController.instance.masterSlider.value;
            OptionDB.instance.musicMixerValue = AudioMixerController.instance.musicSlider.value;
            OptionDB.instance.sfxMixerValue = AudioMixerController.instance.sfxSlider.value;
            OptionDB.instance.teamMixerValue = AudioMixerController.instance.teamSlider.value;
            OptionDB.instance.monsterMixerValue = AudioMixerController.instance.monsterSlider.value;
            OptionDB.instance.uiMixerValue = AudioMixerController.instance.uiSlider.value;
            OptionDB.instance.uiScale = uiCanvas.scaleFactor;
            
            gameObject.SetActive(false);
        }
        
        public void UIOn()
        {
            Debug.Log("눌림");
            if(this.gameObject.activeSelf == false)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }
}
