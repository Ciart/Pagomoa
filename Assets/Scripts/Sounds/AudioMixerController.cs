using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.UI.Title;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Sounds
{
    public struct OptionData
    {
        public float masterMixerValue;
        public float musicMixerValue;
        public float sfxMixerValue;
        public float playerMixerValue;
        public float monsterMixerValue;
        public float uiMixerValue;
    }
    
    public class AudioMixerController : SingletonMonoBehaviour<AudioMixerController>
    {
        [SerializeField] private AudioMixer _audioMixer;
        private OptionData optionData;

        public void InitAudioMixer()
        {
            _audioMixer.GetFloat("Master", out optionData.masterMixerValue);
            _audioMixer.GetFloat("Music", out optionData.musicMixerValue);
            _audioMixer.GetFloat("Sfx", out optionData.sfxMixerValue);
            _audioMixer.GetFloat("Player", out optionData.playerMixerValue);
            _audioMixer.GetFloat("Monster", out optionData.monsterMixerValue);
            _audioMixer.GetFloat("UI", out optionData.uiMixerValue);
        }
        public OptionData GetOptionData() { return optionData; }
        
        public void SetOptionData(OptionData changedData)
        {
            optionData.masterMixerValue = changedData.masterMixerValue;
            optionData.musicMixerValue = changedData.musicMixerValue;
            optionData.sfxMixerValue = changedData.sfxMixerValue;
            optionData.playerMixerValue = changedData.playerMixerValue;
            optionData.monsterMixerValue = changedData.monsterMixerValue;
            optionData.uiMixerValue = changedData.uiMixerValue;
        }
        
        public void SetMasterVolume(float volume)
        {
           _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        }
        public void SetSfxVolume(float volume)
        {
            _audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        }
        public void SetPlayerVolume(float volume)
        {
            _audioMixer.SetFloat("Player", Mathf.Log10(volume) * 20);
        }
        public void SetMonsterVolume(float volume)
        {
            _audioMixer.SetFloat("Monster", Mathf.Log10(volume) * 20);
        }
        public void SetUIVolume(float volume)
        {
            _audioMixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        }
    }
}
