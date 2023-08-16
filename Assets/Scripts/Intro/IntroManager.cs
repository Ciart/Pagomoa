using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Intro
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private int firstDialogId = 2;
        [SerializeField] private int secondDialogId = 3;

        private PlayableDirector _director;
        
        void Start()
        {
            _director = GetComponent<PlayableDirector>();
        }

        public void PlayDialogClip()
        {
            _director.Pause();

            var dialogManager = DialogueManager.Instance;
            var progressDialog = dialogManager.ConversationProgress(firstDialogId);
            
            if (!progressDialog) _director.Play();  
        }
    }    
}

