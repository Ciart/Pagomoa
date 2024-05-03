using UnityEngine;

namespace Ciart.Pagomoa.Sounds
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
        public AudioClip audioClip;
        public float volume;
    }
}
