using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class Option : MonoBehaviour
    {
        static public Option instance = null;

        [SerializeField] public SetAudio audio;
 

        [SerializeField] public TMP_Dropdown dropdown;
        [SerializeField] public Canvas canvas;
        int currentOption = 0;
   

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            MakeDropDown();
            dropdown.onValueChanged.AddListener(delegate { SetDropDown(dropdown.value); });

            FirstOption();
        }
        public void MakeDropDown()
        {
            dropdown.ClearOptions();
            for (int i = 1; i <= 8; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData($"{i}"));
            }
        }
        public void FirstOption()
        {
            if (OptionDB.instance.scale == 1)
            {
                dropdown.value = currentOption;
                SetDropDown(currentOption);
            }
            else
                dropdown.value = OptionDB.instance.scale - 1;
        }
        public void OnOptionUI()
        {
            bool activeOption = false;

            if (gameObject.activeSelf == false)
                activeOption = !activeOption;
        
            gameObject.SetActive(activeOption);

            if(audio != null) 
                OptionDB.instance.audioValue = audio.audioSlider.value;

        }
        public void OffOptionUI()
        {
            bool activeOption = false;
            OptionDB.instance.scale = dropdown.value + 1;
            AudioSlider.instance.SaveAudioValue();
        
            gameObject.SetActive(activeOption);
        }
        public void SetDropDown(int index)
        {
            canvas.GetComponent<CanvasScaler>().scaleFactor = index + 1;
        }
        public void SoundOptionCancle()
        {
            if (audio != null)
            {
                audio.audioMixer.SetFloat("BGM", OptionDB.instance.audioValue);
                audio.audioSlider.value = OptionDB.instance.audioValue;
            }
            OptionDB.instance.scale -= 1;
            dropdown.value = OptionDB.instance.scale;
        
            gameObject.SetActive(false);
        }
    }
}
