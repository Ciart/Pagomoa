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
        public bool isPlayed;
        
        private PlayableDirector _director;
        private float _time;

        void Start()
        {
            _director = GetComponent<PlayableDirector>();
        }

        public void PlayIntro() { 
            _director.Play();
            isPlayed = true;
        }
    }    
}

