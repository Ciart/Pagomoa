using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa.Timelines
{
    [RequireComponent(typeof(SignalReceiver))]
    public abstract class CutScene : MonoBehaviour
    {
        [SerializeField] private PlayableAsset timelineClip;
        [SerializeField] private List<Sprite> portraits;
        [SerializeField] private List<TextAsset> dialogues;

        public PlayableAsset GetTimelineClip() { return timelineClip; }
        public Sprite[] GetSprites() { return portraits.ToArray(); }
        public TextAsset[] GetDialogues() { return dialogues.ToArray(); }
        
        private SignalReceiver _signalReceiver;
        
        void Start() { _signalReceiver = GetComponent<SignalReceiver>(); }

        public SignalReceiver GetSignalReceiver() { return _signalReceiver; }
        public void StartCutSceneDialogue(TextAsset story) { DialogueManager.instance.StartStory(story); }
        
        public abstract void SetInstanceCharacter();
        public abstract void SetBinding(PlayableDirector director);
    }
}