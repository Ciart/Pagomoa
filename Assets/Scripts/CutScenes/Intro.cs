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
        private PlayableDirector _director;
        private float _time;

        void Start()
        {
            _director = GetComponent<PlayableDirector>();
        }

        public void StartIntro()
        {
            if (_director.initialTime != 0f) return;
            
            _director.Play();
        }
        
        private void Update()
        {
            if (_director.state == PlayState.Playing) return;
            
            _time += Time.deltaTime;

            if (_time >= 5f)
            {
                _time = 0f;
                Debug.Log("8초 지남 대화 스킵");
            }
            /*if (_director.state == PlayState.Playing) return;
            
            _time += Time.deltaTime;

            if (_time >= 5f)
            {
                _time = 0f;
                //if (!DialogueManager.Instance.ConversationProgress(_nowScenario))
                    _director.Play();
            }
            
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                _time = 0f;
                //if (!DialogueManager.Instance.ConversationProgress(_nowScenario))
                    _director.Play();
            }*/
        }
    }    
}

