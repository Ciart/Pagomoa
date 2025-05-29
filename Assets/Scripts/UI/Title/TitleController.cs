using System;
using System.Collections;
using System.Threading.Tasks;
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

        [SerializeField] private GameObject _cutSceneCredit;
        [SerializeField] private GameObject _introSprite;
        
        private void Start()
        {
            DataManager.Instance.LoadGameData();
            SceneManager.activeSceneChanged += QuitToTitle;
            DontDestroyOnLoad(_cutSceneCredit);
            DontDestroyOnLoad(_introSprite);
        }

        public async void StartGame(bool isContinue)
        {
            if (isContinue)
            {
                await SaveSystem.Instance.Load(false);
                StartGame();
                return;
            }

            Game.Instance.UI.titleUI.gameObject.SetActive(false);
            
            foreach (var backGround in backGrounds)
            {
                backGround.needScrollDown = true;
            }
            StartCoroutine(StartIntro(backGrounds[0].decreaseDuration));
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

        public void StartGame()
        {
            DataBase.data.GetCutSceneController().GetDirector().Stop();
            SceneManager.LoadSceneAsync("Scenes/WorldScene");
        }

        private IEnumerator StartIntro(float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return backGrounds[0].startIntro;
            DataBase.data.GetCutSceneController().PlayCutScene(_introCutScene);
        }

        private void QuitToTitle(Scene scene, Scene loadScene)
        {
            const int game = 1;
            const int title = 0;
            
            if (loadScene.buildIndex == title)
            {
                if (!backGrounds[0])
                    backGrounds[0] = Instantiate(backGroundUPPrefab);
                if (!backGrounds[1])
                    backGrounds[1] = Instantiate(backGroundDownPrefab);
            }
            else if (loadScene.buildIndex == game)
            {
                foreach (var backGround in backGrounds)
                    Destroy(backGround.gameObject); 
            }
            
            _cutSceneCredit.gameObject.SetActive(!_cutSceneCredit.gameObject.activeSelf);
            _introSprite.gameObject.SetActive(!_introSprite.gameObject.activeSelf);
        }
    }
}
