using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioSource[] sfxSources;
        
        public void Play(MusicBundle music)
        {
            musicSource.clip = music.intro;
            musicSource.Play();
        }
        public void Play(SfxBundle music, Vector3? position = null)
        {

        }
    }
}
