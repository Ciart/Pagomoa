using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    [CreateAssetMenu (fileName ="MusicBundle", menuName ="MuselicBundle")]
    public class MusicBundle : ScriptableObject
    {
        public AudioClip intro;
        public AudioClip loop;
        public float volume;
    }
}
