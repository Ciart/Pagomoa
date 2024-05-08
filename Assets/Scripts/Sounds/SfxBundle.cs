using UnityEngine;

namespace Ciart.Pagomoa.Sounds
{
    public enum SfxType
    {
        TeamEffect,
        MonsterEffect,
        UIEffect
    }
    [CreateAssetMenu(fileName = "New SfxBundle", menuName = "New SfxBundle")]
    public class SfxBundle : ScriptableObject
    {
        public SfxType type;
        public AudioClip[] audioClip;
        public float volume;
    }
}
