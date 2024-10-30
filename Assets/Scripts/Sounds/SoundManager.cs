using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Sounds
{
    public class SoundManager : PManager
    {
        public static SoundManager instance { get; private set; }
        public SoundManager() { instance ??= this; }
        
        public AudioSource musicSource;
        
        private int _loopStartSamples;
        private int _loopEndSamples;
        private int _loopLengthSamples;
        
        public override void Start()
        {
            instance.musicSource = DataBase.data.GetAudioSource();
            PlayMusic("WorldMusic");
        }
        
        public override void Update() // BGM Loop 시점 감지
        {
            if (musicSource.timeSamples >= _loopEndSamples)
            {
                musicSource.timeSamples -= _loopLengthSamples;
                musicSource.Play();
            }
        }
        public void PlayMusic(string bundleName)// 배경음악 재생
        {
            musicSource.Stop();
            MusicBundle bundle = FindMusicBundle(bundleName);
            
            musicSource.clip = bundle.music;

             _loopStartSamples = (int)(bundle.loopStartTime * musicSource.clip.frequency);
             _loopEndSamples = (int)(bundle.loopEndTime * musicSource.clip.frequency);
             _loopLengthSamples = _loopEndSamples - _loopStartSamples;
             
            musicSource.Play();
        }
        public void PlaySfx(string bundleName, bool duplication, Vector3? position = null) // 효과음 재생
        {
            SfxBundle bundle = FindSfxBundle(bundleName);
            if (position == null) // SfxSources 자식들한테서 재생
            {
                PlaySfxBundle(bundle, duplication);
            }
            else // 특정 position에서 소리 재생
            {
                PlaySfxBundlePosition(bundle, position);
            }
        }
        private void PlaySfxBundle(SfxBundle bundle, bool duplication)
        { 
            int random = RandomClip(bundle);
            if (duplication)
            {
                switch (bundle.type)
                {
                    case SfxType.MonsterEffect:
                        FindAudioSource("MonsterEffect").PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.TeamEffect:
                        FindAudioSource("TeamEffect").PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.UIEffect:
                        FindAudioSource("UIEffect").PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.DrillSpinEffect:
                        FindAudioSource("DrillSpinEffect").PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.DrillHitEffect:
                        FindAudioSource("DrillHitEffect").PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                }
            }
            else
            {
                switch (bundle.type)
                {
                    case SfxType.MonsterEffect:
                        FindAudioSource("MonsterEffect").clip = bundle.audioClip[random];
                        FindAudioSource("MonsterEffect").volume = bundle.volume;
                        FindAudioSource("MonsterEffect").Play();
                        break;
                    case SfxType.TeamEffect:
                        FindAudioSource("TeamEffect").clip = bundle.audioClip[random];
                        FindAudioSource("TeamEffect").volume = bundle.volume;
                        FindAudioSource("TeamEffect").Play();
                        break;
                    case SfxType.UIEffect:
                        FindAudioSource("UIEffect").clip = bundle.audioClip[random];
                        FindAudioSource("UIEffect").volume = bundle.volume;
                        FindAudioSource("UIEffect").Play();
                        break;
                    case SfxType.DrillSpinEffect:
                        FindAudioSource("DrillSpinEffect").clip = bundle.audioClip[random];
                        FindAudioSource("DrillSpinEffect").volume = bundle.volume;
                        FindAudioSource("DrillSpinEffect").Play();                        
                        break;
                }
            }
        }
        private void PlaySfxBundlePosition(SfxBundle bundle, Vector3? position)
        {
            int random = RandomClip(bundle);  
            AudioSource.PlayClipAtPoint(bundle.audioClip[random], position.GetValueOrDefault(), FindAudioSource("TeamEffect").volume);
        }
        public AudioSource FindAudioSource(string audioSourceName)
        {
            AudioSource sfxSource = Array.Find(DataBase.data.GetSfxSources(), source => source.gameObject.name == $"{audioSourceName}");
            return sfxSource;
        }
        private MusicBundle FindMusicBundle(string bundleName)
        {
            MusicBundle musicBundle =
                Array.Find(AudioClipDB.instance.MusicBundleDB, bundle => bundle.name == bundleName);
            return musicBundle;
        }
        public SfxBundle FindSfxBundle(string bundleName)
        {
            SfxBundle sfxBundle = 
                Array.Find(AudioClipDB.instance.SfxBundleDB, bundle => bundle.name == bundleName);
            return sfxBundle;
        }
        private int RandomClip(SfxBundle bundle)
        {
            if (bundle.audioClip.Length >= 2)
            {
                int random = Random.Range(0, bundle.audioClip.Length);
                return random;
            }
            else
            {
                return 0;
            }
        }
    }
}
