using System;
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
    
    [RequireComponent(typeof(AudioClips))]
    public class AudioMixerController : MonoBehaviour
    {
        [SerializeField] private AudioClips _clips;
        [SerializeField] private AudioMixer _mixer;
        private OptionData optionData;
        public AudioClips GetAudioClips() { return _clips;}
        public AudioMixer GetAudioMixer() { return _mixer;}
        
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _playerEffect;
        [SerializeField] private AudioSource _monsterEffect;
        [SerializeField] private AudioSource _drillSpinEffect;
        [SerializeField] private AudioSource _drillHitEffect;
        [SerializeField] private AudioSource _UIEffect;
        
        public AudioSource GetMusicSource() { return _music; }
        public AudioSource GetPlayerSource() { return _playerEffect; }
        public AudioSource GetMonsterSource() { return _monsterEffect; }
        public AudioSource GetDrillSpinSource() { return _drillSpinEffect; }
        public AudioSource GetDrillHitSource() { return _drillHitEffect; }
        public AudioSource GetUISource() { return _UIEffect; }

        private void Awake()
        {
            if (Game.Instance.Sound.controller)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public void InitAudioMixer()
        {
            _mixer.GetFloat("Master", out optionData.masterMixerValue);
            _mixer.GetFloat("Music", out optionData.musicMixerValue);
            _mixer.GetFloat("Sfx", out optionData.sfxMixerValue);
            _mixer.GetFloat("Player", out optionData.playerMixerValue);
            _mixer.GetFloat("Monster", out optionData.monsterMixerValue);
            _mixer.GetFloat("UI", out optionData.uiMixerValue);
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
           _mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
        public void SetMusicVolume(float volume)
        {
            _mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        }
        public void SetSfxVolume(float volume)
        {
            _mixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        }
        public void SetPlayerVolume(float volume)
        {
            _mixer.SetFloat("Player", Mathf.Log10(volume) * 20);
        }
        public void SetMonsterVolume(float volume)
        {
            _mixer.SetFloat("Monster", Mathf.Log10(volume) * 20);
        }
        public void SetUIVolume(float volume)
        {
            _mixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        }
    }
}
