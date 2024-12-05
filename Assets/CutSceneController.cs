using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Timelines;
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
        
        private PlayableDirector _director;
        private SignalReceiver _signalReceiver;
        
        public List<CutScene> cutScenes;
        private CutScene _targetCutScene;

        private Transform _camTransform;
        
        [SerializeField] private float fadeDelay;
        
        private LayerMask cutSceneMasks => LayerMask.GetMask("CutScene", "BackGround", "Platform", "Light", "DialogueUI");
        private LayerMask inGameMasks => LayerMask.GetMask("Default", "Entity", "BackGround", "Platform", "Light", "Player", "Ignore Raycast", "UI", "DialogueUI");
        
        // UIManager에 스킵 UI 생성
        // Fadeout 이후 director Play
        // 카메라 초기 위치 설정

        public CutScene GetOnPlayingCutScene()
        {
            return _targetCutScene;
        }
        
        public void StartCutScene(CutScene cutScene)
        {
            UIManager.instance.fadeUI.Fade(FadeFlag.FadeIn);
            
            var player = GameManager.instance.player;
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);    
            }
            player.gameObject.SetActive(false);
            
            
            CutSceneCameraSetting();

            _targetCutScene = cutScene;
            PlayCutScene();
        }
        
        public void EndCutScene()
        {
            _targetCutScene.ClearActors();
            _director.Stop();
            DefaultCameraSetting();
            
            var player = GameManager.instance.player;
            player.gameObject.SetActive(true);
            
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(true);    
            }
            
            UIManager.instance.fadeUI.Fade(FadeFlag.FadeOut);
        }
        
        private void Start()
        {
            _director = GetComponent<PlayableDirector>();
            _signalReceiver = GetComponent<SignalReceiver>();
            
            mainCamera = Camera.main;
        }
        
        public SignalReceiver GetSignalReceiver()
        {
            return _signalReceiver;
        }
        
        private void CutSceneCameraSetting() { mainCamera.cullingMask= cutSceneMasks; }
        private void DefaultCameraSetting() { mainCamera.cullingMask= inGameMasks; }
        
        public bool CutSceneIsPlayed()
        {
            return _director.state == PlayState.Playing;
        }
        
        private void PlayCutScene()
        {
            UIManager.instance.fadeUI.Fade(FadeFlag.FadeInOut);
            UIManager.instance.DeActiveUI();
               
            foreach (var cutScene in cutScenes)
            {
                if (cutScene.name.Trim() == _targetCutScene.name.Trim())
                {
                    _targetCutScene.SetCutSceneController(this);
                    _targetCutScene.SetBinding(_director);
                    _targetCutScene.SetInstanceCharacter();

                    _director.playableAsset = _targetCutScene.GetTimelineClip();
                    _director.timeUpdateMode = DirectorUpdateMode.GameTime;
                    
                    break;
                }    
            }
            
            foreach (var actor in _targetCutScene.GetActors())
            {
                var chatBalloon = actor.GetComponent<ChatBalloon>().balloon;
                chatBalloon.SetActive(false);
            }
            
            _director.Play();
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification.id == "0")
            {
                var dialogue = notification as DialogueMarker;
                
                if (dialogue != null)
                    DialogueManager.instance.StartStory(dialogue.story);    
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
                        _targetCutScene.IncreaseChatIndex();
                        return;
                    }
                    else if (chat.content.Trim() != "") 
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
