using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Sounds
{
    public class AudioClipDB : SingletonMonoBehaviour<AudioClipDB>
    {
        [SerializeField] private MusicBundle[] musicBundleDB;
        [SerializeField] private SfxBundle[] sfxBundleDB;
        
        
        public MusicBundle[] MusicBundleDB
        {
            get { return musicBundleDB; }
        }
        
        public SfxBundle[] SfxBundleDB
        {
            get { return sfxBundleDB; }
        }
    }
}
