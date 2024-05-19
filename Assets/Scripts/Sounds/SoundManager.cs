using System;
using Ciart.Pagomoa.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Sounds
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        public AudioSource musicSource;
        public AudioSource[] sfxSources;
        
        private int _loopStartSamples;
        private int _loopEndSamples;
        private int _loopLengthSamples;
        
        private void Start()
        {
            // Init();
        }
        
        private void Update() // BGM Loop 시점 감지
        {
            if (musicSource.timeSamples >= _loopEndSamples)
                musicSource.timeSamples -= _loopLengthSamples;
        }
        
        private void Init() // SfxBundle의 Type의 개수만큼 생성
        {
            string[] soundNames = Enum.GetNames(typeof(SfxType));
            for (int i = 0; i < soundNames.Length; i++)
            {
                GameObject audioSource = new GameObject { name = soundNames[i] };
                sfxSources[i] = audioSource.AddComponent<AudioSource>();
                audioSource.transform.parent = this.transform;
            }
        }
        public void PlayMusic(string bundleName)// 배경음악 재생
        {
            MusicBundle bundle = FindMusicBundle(bundleName);
            
            _loopStartSamples = (int)(bundle.loopStartTime * musicSource.clip.frequency);
            _loopEndSamples = (int)(bundle.loopEndTime * musicSource.clip.frequency);
            _loopLengthSamples = _loopEndSamples * _loopStartSamples;
            
            musicSource.clip = bundle.music;
            musicSource.Play();
        }
        public void PlaySfx(string bundleName, Vector3? position = null) // 효과음 재생
        {
            SfxBundle bundle = FindSfxBundle(bundleName);
            if (position == null) // SfxSources 자식들한테서 재생
            {
                PlaySfxBundle(bundle);
            }
            else // 특정 position에서 소리 재생
            {
                PlaySfxBundlePosition(bundle, position);
            }
        }
        private void PlaySfxBundle(SfxBundle bundle)
        { 
            int random = RandomClip(bundle);
            
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
            }
        }
        private void PlaySfxBundlePosition(SfxBundle bundle, Vector3? position)
        {
            int random = RandomClip(bundle);  
            AudioSource.PlayClipAtPoint(bundle.audioClip[random], position.GetValueOrDefault(), FindAudioSource("TeamEffect").volume);
        }
        public AudioSource FindAudioSource(string indexName)
        {
            AudioSource sfxSource = Array.Find(sfxSources, source => source.gameObject.name == $"{indexName}");
            return sfxSource;
        }
        private MusicBundle FindMusicBundle(string bundleName)
        {
            MusicBundle musicBundle =
                Array.Find(AudioClipDB.instance.MusicBundleDB, bundle => bundle.name == bundleName);
            return musicBundle;
        }
        private SfxBundle FindSfxBundle(string bundleName)
        {
            SfxBundle sfxBundle = 
                Array.Find(AudioClipDB.instance.SfxBundleDB, bundle => bundle.name == bundleName);
            return sfxBundle;
        }
        private int RandomClip(SfxBundle bundle)
        {
            Debug.Log(bundle.name);
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
