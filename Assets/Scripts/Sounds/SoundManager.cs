﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Sounds
{
    public class SoundManager : Manager<SoundManager>
    {
        private AudioMixerController _controller => DataBase.data.GetAudioController();
        public AudioMixerController controller => _controller;

        private int _loopStartSamples;
        private int _loopEndSamples;
        private int _loopLengthSamples;

        public override void Start()
        {
            PlayMusic("WorldMusic");
            controller.GetMusicSource().Pause();
        }
        
        public override void Update() // BGM Loop 시점 감지
        {
            if (controller.GetMusicSource().isPlaying == false) return;
            if (controller.GetMusicSource().timeSamples >= _loopEndSamples)
            {
                controller.GetMusicSource().timeSamples -= _loopLengthSamples;
                controller.GetMusicSource().Play();
            }
        }
        public void PlayMusic(string bundleName)// 배경음악 재생
        {
            controller.GetMusicSource().Stop();
            MusicBundle bundle = FindMusicBundle(bundleName);
            
            controller.GetMusicSource().clip = bundle.music;
            
             _loopStartSamples = (int)(bundle.loopStartTime * controller.GetMusicSource().clip.frequency);
             _loopEndSamples = (int)(bundle.loopEndTime * controller.GetMusicSource().clip.frequency);
             _loopLengthSamples = _loopEndSamples - _loopStartSamples;
             
             controller.GetMusicSource().Play();
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
                        controller.GetMonsterSource().PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.PlayerEffect:
                        controller.GetPlayerSource().PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.UIEffect:
                        controller.GetUISource().PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.DrillSpinEffect:
                        controller.GetDrillSpinSource().PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                    case SfxType.DrillHitEffect:
                        controller.GetDrillHitSource().PlayOneShot(bundle.audioClip[random], bundle.volume);
                        break;
                }
            }
            else
            {
                switch (bundle.type)
                {
                    case SfxType.MonsterEffect:
                        controller.GetMonsterSource().clip = bundle.audioClip[random];
                        controller.GetMonsterSource().volume = bundle.volume;
                        controller.GetMonsterSource().Play();
                        break;
                    case SfxType.PlayerEffect:
                        controller.GetPlayerSource().clip = bundle.audioClip[random];
                        controller.GetPlayerSource().volume = bundle.volume;
                        controller.GetPlayerSource().Play();
                        break;
                    case SfxType.UIEffect:
                        controller.GetUISource().clip = bundle.audioClip[random];
                        controller.GetUISource().volume = bundle.volume;
                        controller.GetUISource().Play();
                        break;
                    case SfxType.DrillSpinEffect:
                        controller.GetDrillSpinSource().clip = bundle.audioClip[random];
                        controller.GetDrillSpinSource().volume = bundle.volume;
                        controller.GetDrillSpinSource().Play();                        
                        break;
                }
            }
        }
        private void PlaySfxBundlePosition(SfxBundle bundle, Vector3? position)
        {
            int random = RandomClip(bundle);  
            AudioSource.PlayClipAtPoint(bundle.audioClip[random], position.GetValueOrDefault(), controller.GetPlayerSource().volume);
        }
        private MusicBundle FindMusicBundle(string bundleName)
        {
            MusicBundle musicBundle =
                Array.Find(controller.GetAudioClips().MusicBundleDB, bundle => bundle.name == bundleName);
            return musicBundle;
        }
        public SfxBundle FindSfxBundle(string bundleName)
        {
            SfxBundle sfxBundle = 
                Array.Find(controller.GetAudioClips().SfxBundleDB, bundle => bundle.name == bundleName);
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
