using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Timelines;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa
{
    public enum StreamName
    {
        Pago,
        Moa,
        Shopkeeper,
        Signal, 
    }
    public class CutSceneController : MonoBehaviour, INotificationReceiver
    {
        public Camera mainCamera;
        public CinemachineBrain mainCinemachine;
        public List<CinemachineVirtualCamera> cameras;
        
        private PlayableDirector _director;
        private SignalReceiver _signalReceiver;
        
        public List<CutSceneTrigger> triggers = new List<CutSceneTrigger>();
        private CutScene? _targetCutScene;
        private Transform _camTransform;
        private CutSceneTrigger? _currentCutSceneTrigger;
        public void SetCutSceneTrigger(CutSceneTrigger trigger) { _currentCutSceneTrigger = trigger; }
        
        
        [SerializeField] private float fadeDelay;
        
        private LayerMask CutSceneMasks => LayerMask.GetMask("CutScene", "BackGround", "Platform", "Light", "DialogueUI");
        private LayerMask InGameMasks => LayerMask.GetMask("Default", "Entity", "BackGround", "Platform", "Light", "Player", "Ignore Raycast", "UI", "DialogueUI");
        
        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
            _signalReceiver = GetComponent<SignalReceiver>();

            mainCamera = Camera.main;
            mainCinemachine = mainCamera.GetComponent<CinemachineBrain>(); 
        }
        
        private void Start()
        {
            _director.stopped += EndCutScene;

            foreach (var trigger in triggers)
            {
                Game.Instance.Time.RegisterTickEvent(trigger.OnCutSceneTrigger);
            }
        }

        private void OnPaused(PausedEvent e)
        {
            _director.Pause();
        }

        private void OnResumed(ResumedEvent e)
        {
            _director.Resume();
        }

        private void OnEnable()
        {
            EventManager.AddListener<PausedEvent>(OnPaused);
            EventManager.AddListener<ResumedEvent>(OnResumed);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PausedEvent>(OnPaused);
            EventManager.RemoveListener<ResumedEvent>(OnResumed);
        }

        public CutScene GetOnPlayingCutScene()
        {
            return _targetCutScene;
        }
        
        public void StartCutScene(CutScene cutScene)
        {
            Game.Instance.UI.DeActiveUI();
            
            var player = Game.Instance.player;
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);    
            }
            player.gameObject.SetActive(false);
            
            CutSceneCameraSetting();
            
            _targetCutScene = cutScene;
            PlayCutScene(_targetCutScene);
        }
        
        private void EndCutScene(PlayableDirector director)
        {
            _targetCutScene.ClearActors();
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeOut, fadeDelay);
            DefaultCameraSetting();
            
            var player = Game.Instance.player;
            player.gameObject.SetActive(true);
            Game.Instance.UI.ActiveUI();
            
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(true);    
            }
            
            _currentCutSceneTrigger.OffCutSceneTrigger();

            _targetCutScene = null;
            _currentCutSceneTrigger = null;
        }
        
        public SignalReceiver GetSignalReceiver()
        {
            return _signalReceiver;
        }
        
        private void CutSceneCameraSetting() { mainCamera.cullingMask= CutSceneMasks; }
        private void DefaultCameraSetting() { mainCamera.cullingMask= InGameMasks; }
        
        public bool CutSceneIsPlayed()
        {
            return _director.state == PlayState.Playing;
        }

        public bool RePlayCutScene()
        {
            if (_director.state != PlayState.Paused) return false;
            
            _director.Play();
            return true;
        }
        
        public void PlayCutScene(CutScene cutScene)
        {
            if (!_targetCutScene) _targetCutScene = cutScene;
                
            _targetCutScene.SetCutSceneController(this);
            _targetCutScene.SetBinding(_director);
            _targetCutScene.SetInstanceCharacter();

            _director.playableAsset = _targetCutScene.GetTimelineClip();
            _director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            
            foreach (var actor in _targetCutScene.GetActors())
            {
                actor.TryGetComponent<ChatBalloon>(out var chat);
                chat.balloon.SetActive(false);
            }
            
            _director.Play();
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification.id == "0")
            {
                var dialogue = notification as DialogueMarker;

                if (dialogue != null)
                {
                    _director.Pause();
                    Game.Instance.Dialogue.StartStory(dialogue.story);
                }
            }
            else if (notification.id == "1")
            {
                var chat = notification as ChatMarker;

                if (chat == null) return;
                
                foreach (var actor in _targetCutScene.GetActors())
                {
                    var targetName = chat.targetTalker.name + "(Clone)";
                    
                    if (targetName != actor.name) continue;
                    
                    if (chat.content.Trim() == "")
                    {
                        var target = actor.GetComponent<Chat>();
                        
                        target.Chatting(_targetCutScene.GetMiniChat(), chat.duration);
                        _targetCutScene.IncreaseMiniChatIndex();
                            
                        return;
                    }
                    if (chat.content.Trim() != "") 
                    {
                        var target = actor.GetComponent<Chat>();
                        
                        target.Chatting(chat.content, chat.duration);
                        return;
                    }
                }

            }
        }
    }
}
