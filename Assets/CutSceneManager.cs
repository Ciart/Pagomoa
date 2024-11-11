using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Timelines;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

namespace Ciart.Pagomoa
{
    public class CutSceneManager : SingletonMonoBehaviour<CutSceneManager>
    {
        public Camera mainCamera;
        
        private CinemachineVirtualCamera _focusCamera;
        
        private PlayableDirector _director;
        
        public List<CutScene> cutScenes;
        private CutScene _targetCutScene;

        [SerializeField] private float fadeDelay; 
        
        // UIManager에 스킵 UI 생성
        
        // Fadeout 이후 director Play
        // 카메라 초기 위치 설정
        // mini chat 구성
        // 카메라로 둘을 비추기
        // Dialogue 삽입

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                EventManager.Notify(new PlayCutScene("ShopkeeperCutScene"));
                CutSceneCameraSetting();
            }
        }

        private void Start()
        {
            _director = GetComponent<PlayableDirector>();

            _focusCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            
            mainCamera = Camera.main;
        }
        
        private void OnEnable() { EventManager.AddListener<PlayCutScene>(PlayCutScene); }

        private void OnDisable() { EventManager.RemoveListener<PlayCutScene>(PlayCutScene); }


        public void CutSceneCameraSetting()
        {
            mainCamera.cullingMask = LayerMask.GetMask("CutScene", "BackGround", "Platform", "Light");
        }

        public void DefaultCameraSetting()
        {
            mainCamera.cullingMask = LayerMask.GetMask("Default", "Entity", "BackGround", "Platform", "Light", "Player", "Ignore Raycast", "UI");
        }
        
        public bool CutSceneIsPlayed()
        {
            return _director.state == PlayState.Playing;
        }

        public CutScene GetTargetCutSceneData()
        {
            return _targetCutScene;
        }

        public void DelayPlayTime()
        {
            _director.playableGraph.SetTimeUpdateMode(0);
        } 
        
        private void PlayCutScene(PlayCutScene e)
        {
            StartCoroutine(Fade());
            
            TimeManager.instance.PauseTime();
            
            foreach (var cutScene in cutScenes)
            {
                if (cutScene.name.Trim() == e.cutSceneName.Trim())
                {
                    _targetCutScene = cutScene;
                    
                    _targetCutScene.SetBinding(_director);
                    _targetCutScene.SetInstanceCharacter();

                    _director.playableAsset = _targetCutScene.GetTimelineClip();
                    
                    return;
                }    
            }
        }

        private IEnumerator Fade()
        {
            EventManager.Notify(new FadeEvent(FadeState.FadeIn));
            
            yield return new WaitForSecondsRealtime(fadeDelay);
            
            EventManager.Notify(new FadeEvent(FadeState.FadeOut));
            
            yield return new WaitForSecondsRealtime(fadeDelay);
            
            _director.Play();
        }
    }
}
