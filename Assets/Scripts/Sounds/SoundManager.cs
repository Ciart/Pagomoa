using System;
using Ciart.Pagomoa.Systems;
using UnityEngine;
using Object = UnityEngine.Object;
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
            Init();
        }
        
        private void Update() // BGM Loop 시점 감지
        {
            if (musicSource.timeSamples >= _loopEndSamples)
                musicSource.timeSamples -= _loopLengthSamples;
        }
        
        private void Init() // SfxBundle의 Type의 개수만큼 생성
        {
            GameObject root = GameObject.Find("SfxSources");
            if (root == null)
            {
                root = new GameObject { name = "SfxSources" };
                Object.DontDestroyOnLoad(root);
                
                string[] soundNames = Enum.GetNames(typeof(SfxType));
                for (int i = 0; i < soundNames.Length; i++)
                {
                    GameObject audioSource = new GameObject { name = soundNames[i] };
                    sfxSources[i] = audioSource.AddComponent<AudioSource>();
                    audioSource.transform.parent = root.transform;
                }
            }
        }
        public void Play(MusicBundle bundle)// 배경음악 재생
        {
            _loopStartSamples = (int)(bundle.loopStartTime * musicSource.clip.frequency);
            _loopEndSamples = (int)(bundle.loopEndTime * musicSource.clip.frequency);
            _loopLengthSamples = _loopEndSamples * _loopStartSamples;
            
            musicSource.clip = bundle.music;
            musicSource.Play();
        }
        public void Play(string bundleName, Vector3? position = null) // 효과음 재생
        {
            SfxBundle bundle = FindSfxBundle(bundleName);
            if (position == null) // SfxSources 자식들한테서 재생
            {
                PlaySfxBundle(bundle);
            }
            else // 특정 position에서 소리 재생
            {
                // PlayClipAtPoint(bundle.audioClip, position);
            }
        }
        private void PlaySfxBundle(SfxBundle bundle)
        {
            int random = RandomClip(bundle);
            switch (bundle.type)
            {
                case SfxType.MonsterEffect:
                    FindAudioSource("MonsterEffect").PlayOneShot(bundle.audioClip[random]);
                    break;
                case SfxType.TeamEffect:
                    FindAudioSource("TeamEffect").PlayOneShot(bundle.audioClip[random]);
                    break;
                case SfxType.UIEffect:
                    FindAudioSource("UIEffect").PlayOneShot(bundle.audioClip[random]);
                    break;
            }
        }
        
        public AudioSource FindAudioSource(string indexName)
        {
            AudioSource sfxSource = Array.Find(sfxSources, source => source.gameObject.name == $"{indexName}");
            return sfxSource;
        }
        
        private SfxBundle FindSfxBundle(string bundleName)
        {
            SfxBundle sfxBundle = Array.Find(AudioClipDB.instance.SfxBundleDB, bundle => bundle.name == bundleName);
            return sfxBundle;
        }
        private int RandomClip(SfxBundle bundle)
        {
            int random = Random.Range(0, bundle.audioClip.Length);
            return random;
        }
    }
}
