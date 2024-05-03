using UnityEngine;

namespace Ciart.Pagomoa.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioSource[] sfxSources;
        
        private int _loopStartSamples;
        private int _loopEndSamples;
        private int _loopLengthSamples;
        
        private void Update()
        {
            if (musicSource.timeSamples >= _loopEndSamples)
                musicSource.timeSamples -= _loopLengthSamples;
        }
        public void Play(MusicBundle bundle)
        {
            // 배경음악 재생
            _loopStartSamples = (int)(bundle.loopStartTime * musicSource.clip.frequency);
            _loopEndSamples = (int)(bundle.loopEndTime * musicSource.clip.frequency);
            _loopLengthSamples = _loopEndSamples * _loopStartSamples;
            
            musicSource.clip = bundle.music;
            musicSource.Play();
        }
        public void Play(SfxBundle bundle, Vector3? position = null)
        {
            // 효과음 재생
            if (position == null)
            {
                FindNullSfxSource(bundle);
                // sfxSources[].PlayOneShot();
            }
            else
            {
                FindNullSfxSource((bundle));
                // AudioSorce.PlayClipAtPoint();
            }
        }
        
        private void FindNullSfxSource(SfxBundle bundle)
        {
            foreach (AudioSource sfxSource in sfxSources) // clip이 비어있는 sfxSource 찾기
            {
                if (sfxSource.clip == null)
                    sfxSource.clip = bundle.audioClip;
            }
        }
    }
}
