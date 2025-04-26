using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Sounds
{
    public class AudioClips : MonoBehaviour
    {
        [SerializeField] private MusicBundle[] musicBundleDB;
        [SerializeField] private SfxBundle[] sfxBundleDB;
        
        public MusicBundle[] MusicBundleDB => musicBundleDB;
        public SfxBundle[] SfxBundleDB => sfxBundleDB;
    }
}
