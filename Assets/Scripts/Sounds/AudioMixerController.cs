using Ciart.Pagomoa.Systems;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Sounds
{
    public class AudioMixerController : SingletonMonoBehaviour<AudioMixerController>
    {
        [SerializeField] private AudioMixer audioMixer;
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
        public Slider playerSlider;
        public Slider monsterSlider;
        public Slider uiSlider;

        public void AddSliderFunction()
        {
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            playerSlider.onValueChanged.AddListener(SetTeamVolume);
            monsterSlider.onValueChanged.AddListener(SetMonsterVolume);
            uiSlider.onValueChanged.AddListener(SetUIVolume);
        }
        
        private void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        private void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        }
        private void SetSfxVolume(float volume)
        {
            audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        }
        private void SetTeamVolume(float volume)
        {
            audioMixer.SetFloat("Team", Mathf.Log10(volume) * 20);
        }
        private void SetMonsterVolume(float volume)
        {
            audioMixer.SetFloat("Monster", Mathf.Log10(volume) * 20);
        }
        private void SetUIVolume(float volume)
        {
            audioMixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        }
    }
}
