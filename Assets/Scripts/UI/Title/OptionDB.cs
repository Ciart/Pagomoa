using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;


namespace Ciart.Pagomoa.UI.Title
{
    public class OptionDB : SingletonMonoBehaviour<OptionDB>
    {
        public float masterMixerValue;
        public float musicMixerValue;
        public float sfxMixerValue;
        public float teamMixerValue;
        public float monsterMixerValue;
        public float uiMixerValue;
        public float uiScale;
        
        private void Start()
        {
            masterMixerValue = AudioMixerController.instance.masterSlider.value;
            musicMixerValue = AudioMixerController.instance.musicSlider.value;
            sfxMixerValue = AudioMixerController.instance.sfxSlider.value;
            teamMixerValue = AudioMixerController.instance.teamSlider.value;
            monsterMixerValue = AudioMixerController.instance.monsterSlider.value;
            uiMixerValue = AudioMixerController.instance.uiSlider.value;
        }
    }
}
