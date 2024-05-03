using UnityEngine;

namespace Ciart.Pagomoa.Sounds
{
    [CreateAssetMenu (fileName ="MusicBundle", menuName ="MusicBundle")]
    public class MusicBundle : ScriptableObject
    {
        public AudioClip music;
        public float volume;
        public double loopStartTime;
        public double loopEndTime;
    }
}
