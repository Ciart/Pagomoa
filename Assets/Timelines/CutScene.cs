using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa.Timelines
{
    public abstract class CutScene : MonoBehaviour
    {
        private CutSceneController _cutSceneController;
        
        [SerializeField] private PlayableAsset timelineClip;
        [SerializeField] private List<Sprite> portraits;
        [SerializeField] private List<TextAsset> dialogues;

        [HideInInspector] public List<GameObject> actors;
        
        public string[] miniChats;
        private int _index = 0;
        
        public string GetMiniChat()
        {
            Debug.Log(miniChats.Length);
            Debug.Log(_index);
            Debug.Log(miniChats[_index] + ", " + _index);
            return miniChats[_index];
        }
        public void IncreaseChatIndex() { _index++; }
        public void ClearActors() {
            for (var i = actors.Count - 1; i > 0; i--)
            {
                Destroy(actors[i].gameObject);
                actors.RemoveAt(i);
            }
        }

        public void SetCutSceneController(CutSceneController controller) { _cutSceneController = controller; }
        public CutSceneController GetCutSceneController() { return _cutSceneController; }
        public PlayableAsset GetTimelineClip() { return timelineClip; }
        public Sprite[] GetSprites() { return portraits.ToArray(); }
        public TextAsset[] GetDialogues() { return dialogues.ToArray(); }

        public abstract List<GameObject> GetActors();
        public abstract void SetInstanceCharacter();
        public abstract void SetBinding(PlayableDirector director);
    }
}