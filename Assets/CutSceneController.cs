using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Timelines;
using Unity.VisualScripting;
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
        
        public List<CutSceneTrigger> triggers = new List<CutSceneTrigger>();
        private CutScene? _targetCutScene;
        private CutSceneTrigger? _currentCutSceneTrigger;

        private Transform _camTransform;
        
        [SerializeField] private float fadeDelay;
        
        private LayerMask cutSceneMasks => LayerMask.GetMask("CutScene", "BackGround", "Platform", "Light", "DialogueUI");
        private LayerMask inGameMasks => LayerMask.GetMask("Default", "Entity", "BackGround", "Platform", "Light", "Player", "Ignore Raycast", "UI", "DialogueUI");
        
        private void Start()
        {
            _director = GetComponent<PlayableDirector>();
            _signalReceiver = GetComponent<SignalReceiver>();
            
            mainCamera = Camera.main;
            
            _director.stopped += EndCutScene;
            
            foreach (var trigger in triggers)
            {
                switch (trigger.name)
                {
                    case "ShopKeeperTrigger":
                        _currentCutSceneTrigger = trigger;
                        void WhenCutSceneEnd()
                        {
                            Instantiate(trigger.gameObject, Vector3.zero, Quaternion.identity);
                        }
                        trigger.OnCutSceneTrigger(WhenCutSceneEnd);
                        continue;
                }
            }
        }
        
        public CutScene GetOnPlayingCutScene()
        {
            return _targetCutScene;
        }
        
        public void StartCutScene(CutScene cutScene)
        {
            Game.Instance.UI.DeActiveUI();
            
            var player = Game.instance.player;
            for (int i = 0; i < player.transform.childCount; i++)
            {
                player.transform.GetChild(i).gameObject.SetActive(false);    
            }
            player.gameObject.SetActive(false);
            
            CutSceneCameraSetting();
            
            _targetCutScene = cutScene;
            PlayCutScene();
        }
        
        private void EndCutScene(PlayableDirector director)
        {
            _targetCutScene.ClearActors();
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeOut, fadeDelay);
            DefaultCameraSetting();
            
            Game.Instance.UI.ActiveUI();
            var player = Game.instance.player;
            player.gameObject.SetActive(true);
            
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
        
        private void CutSceneCameraSetting() { mainCamera.cullingMask= cutSceneMasks; }
        private void DefaultCameraSetting() { mainCamera.cullingMask= inGameMasks; }
        
        public bool CutSceneIsPlayed()
        {
            return _director.state == PlayState.Playing;
        }
        
        private void PlayCutScene()
        {
            _targetCutScene.SetCutSceneController(this);
            _targetCutScene.SetBinding(_director);
            _targetCutScene.SetInstanceCharacter();

            _director.playableAsset = _targetCutScene.GetTimelineClip();
            _director.timeUpdateMode = DirectorUpdateMode.GameTime;
            
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
                    Game.Instance.Dialogue.StartStory(dialogue.story);    
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
