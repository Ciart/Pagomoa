using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.CutScenes
{
    public class Intro : MonoBehaviour
    {
        private bool _introIsPlayed;
        
        private PlayableDirector _director;
        private float _time;

        void Start()
        {
            _director = GetComponent<PlayableDirector>();

            _director.paused += FirstDialogue;
        }

        public void StartIntro()
        {
            if (_introIsPlayed) return;

            _introIsPlayed = true;
            
            _director.Play();
        }
        
        private void Update()
        {
            
        }

        private void FirstDialogue(PlayableDirector aDirector)
        {
            
        }

        public void PauseDirector()
        {
            _director.Pause();
        }
    }    
}

