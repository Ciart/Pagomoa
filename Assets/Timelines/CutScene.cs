using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa.Timelines
{
    public abstract class CutScene : MonoBehaviour
    {
        [SerializeField] private PlayableAsset timelineClip;
        [SerializeField] private List<Sprite> portraits;
        [SerializeField] private List<TextAsset> dialogues;

        public PlayableAsset GetTimelineClip() { return timelineClip; }
        public Sprite[] GetSprites() { return portraits.ToArray(); }
        public TextAsset[] GetDialogues() { return dialogues.ToArray(); }
        
        public abstract void SetInstanceCharacter();
        public abstract void SetBinding(PlayableDirector director);
    }
}