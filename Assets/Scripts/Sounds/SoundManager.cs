using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Sounds
{
    public class SoundManager : Manager<SoundManager>
    {
        public AudioSource musicSource;
        
        private int _loopStartSamples;
        private int _loopEndSamples;
        private int _loopLengthSamples;

        private AudioSource _playerEffect;
        private AudioSource _monsterEffect;

        public bool isTitle;
        public override void Start()
        {
            musicSource = DataBase.data.GetAudioSource();
            AudioSource[] sources = DataBase.data.GetSfxSources();
            
            foreach (AudioSource source in sources)
            {
                if (source.gameObject.name == nameof(SfxType.PlayerEffect))
                    _playerEffect = source;
                else if (source.gameObject.name == nameof(SfxType.MonsterEffect))
                    _monsterEffect = source;
            }

            var idx = SceneManager.GetActiveScene().buildIndex;
            if (idx == 1)
            {
                PlayMusic("WorldMusic");
                isTitle = false;
            }
            else
            {
                musicSource.Stop();
                isTitle = true;
            }
        }
        
        public override void Update() // BGM Loop 시점 감지
        {
            if (isTitle) return;
            
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
                        _monsterEffect.PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.PlayerEffect:
                        _playerEffect.PlayOneShot(bundle.audioClip[random], bundle.volume);
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
                        _monsterEffect.clip = bundle.audioClip[random];
                        _monsterEffect.volume = bundle.volume;
                        _monsterEffect.Play();
                        break;
                    case SfxType.PlayerEffect:
                        _playerEffect.clip = bundle.audioClip[random];
                        _playerEffect.volume = bundle.volume;
                        _playerEffect.Play();
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
            AudioSource.PlayClipAtPoint(bundle.audioClip[random], position.GetValueOrDefault(), _playerEffect.volume);
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
