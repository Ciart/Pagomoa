using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Sounds
{
    public class AudioMixerController : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider teamSlider;
        [SerializeField] private Slider monsterSlider;
        [SerializeField] private Slider uiSlider;
        
        
        private void Awake()
        {
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            teamSlider.onValueChanged.AddListener(SetTeamVolume);
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
