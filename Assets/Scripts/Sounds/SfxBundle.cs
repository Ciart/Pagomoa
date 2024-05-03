using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    [CreateAssetMenu(fileName ="New SfxBundle", menuName ="New SfxBundle")]
    public class SfxBundle : ScriptableObject
    {
        public enum SfxType
        {
            Default,
            TeamEffect,
            MonsterEffect
        }
        public AudioClip[] audioClips;
        public float volume;
    }
}
