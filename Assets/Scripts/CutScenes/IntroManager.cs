using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Playables;

namespace Ciart.Pagomoa.CutScenes
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private int firstDialogId = 2;
        [SerializeField] private int secondDialogId = 3;
        [SerializeField] private int thirdDialogId = 4;
        [SerializeField] private int fourthDialogId = 5;

        private PlayableDirector _director;
        private int _nowScenario;
        private float _time;

        void Start()
        {
            _director = GetComponent<PlayableDirector>();
        }

        private void Update()
        {
            if (_director.state == PlayState.Playing) return;
            
            _time += Time.deltaTime;

            if (_time >= 5f)
            {
                _time = 0f;
                if (!DialogueManager.Instance.ConversationProgress(_nowScenario))
                    _director.Play();
            }
            
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                _time = 0f;
                if (!DialogueManager.Instance.ConversationProgress(_nowScenario))
                    _director.Play();
            }
        }

        public void PlayFirstDialogClip()
        {
            _director.Pause();
            _nowScenario = firstDialogId;

            var dialogManager = DialogueManager.Instance;
            var progressDialog = dialogManager.ConversationProgress(_nowScenario);
            
            if (!progressDialog) NoDialogSignal();
        }
        
        public void PlaySecondDialogClip()
        {
            _director.Pause();
            _nowScenario = secondDialogId;
            
            var dialogManager = DialogueManager.Instance;
            var progressDialog = dialogManager.ConversationProgress(_nowScenario);
            
            if (!progressDialog) NoDialogSignal();
        }
        
        public void PlayThirdDialogClip()
        {
            _director.Pause();
            _nowScenario = thirdDialogId;
            
            var dialogManager = DialogueManager.Instance;
            var progressDialog = dialogManager.ConversationProgress(_nowScenario);
            
            if (!progressDialog) NoDialogSignal();
        }
        
        public void PlayFourthDialogClip()
        {
            _director.Pause();
            _nowScenario = fourthDialogId;
            
            var dialogManager = DialogueManager.Instance;
            var progressDialog = dialogManager.ConversationProgress(_nowScenario);

            if (!progressDialog) NoDialogSignal();
        }

        public void IntroEnd()
        {
            _director.Stop();
        }
        
        private void NoDialogSignal()
        {
            Debug.Log("시나리오 없음");
            _director.Play();  
        }

    }    
}

