using System;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa.UI.Title
{
    public class TitleController : SingletonMonoBehaviour<TitleController>
    {
        public bool isFirstStart = false;
        
        public InfiniteScrollBackground[] backGrounds = new InfiniteScrollBackground[2];
        [SerializeField] private InfiniteScrollBackground backGroundUPPrefab;
        [SerializeField] private InfiniteScrollBackground backGroundDownPrefab;
        [SerializeField] private CutScene _introCutScene;
        private bool isIntro = false;
        
        private void Start()
        {
            isIntro = false;
            DataManager.Instance.LoadGameData();
            SceneManager.activeSceneChanged += QuitToTitle;
        }

        public void StartGame(bool isContinue)
        {
            Game.Instance.UI.titleUI.gameObject.SetActive(false);

            if (isContinue)
            {
                PlayerPrefs.SetInt("SaveSlot", 1);
                StartGame();
                return;
            }

            foreach (var backGround in backGrounds)
            {
                backGround.needScrollDown = true;
            }
            // foreach (var backGround in backGrounds)
            // {
            //     backGround.needSlowDownScroll = true;
            //     titlePanel.SetActive(false);
            // }
        }

        public void ReStart()
        {
            DataManager.Instance.DeleteGameData();
            DataManager.Instance.LoadGameData();

            StartGame(true);
        }

        public void PressStartButton()
        {
            isFirstStart = DataManager.Instance.data.introData.isFirstStart;
            if (!isFirstStart)
            {
                transform.Find("StartQuestion").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("LoadQuestion").gameObject.SetActive(true);
            }
        }

        public void PopUpReStart()
        {
            transform.Find("LoadQuestion").gameObject.SetActive(false);
            transform.Find("ReStartQuestion").gameObject.SetActive(true);
        }

        public void EndGame() { Application.Quit(); }
        private void FixedUpdate()
        {
            // 컷씬 시작하면 더이상 작동하지 않음
            if (isIntro) return;
            if (!CutSceneController.Instance.GetDirector()) return;
            if (CutSceneController.Instance.GetDirector().state == PlayState.Playing) return;
            
            if (backGrounds[0].startIntro && CutSceneController.Instance.GetDirector().state != PlayState.Playing)
            {
                CutSceneController.Instance.PlayCutScene(_introCutScene);
                isIntro = true;
            }
        }

        public void StartGame()
        {
            //if (intro.isPlayed) intro.gameObject.SetActive(false);
            CutSceneController.Instance.GetDirector().Stop();
            SceneManager.LoadScene("Scenes/WorldScene");
        }

        private void QuitToTitle(Scene scene, Scene loadScene)
        {
            const int game = 1;
            const int title = 0;
            
            if (loadScene.buildIndex == title)
            {
                if (!backGrounds[0])
                    backGrounds[0] = Instantiate(backGroundUPPrefab, transform);
                if (!backGrounds[1])
                    backGrounds[1] = Instantiate(backGroundDownPrefab, transform);
            }
            else if (loadScene.buildIndex == game)
            {
                isIntro = false;
                foreach (var backGround in backGrounds)
                    Destroy(backGround.gameObject); 
            }
        }
    }
}