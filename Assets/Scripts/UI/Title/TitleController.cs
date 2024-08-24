using System;
using System.Collections;
using Ciart.Pagomoa.CutScenes;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa.UI.Title
{
    public class TitleController : SingletonMonoBehaviour<TitleController>
    {
        public bool isFirstStart = false;

        public InfiniteScrollBackground[] backGrounds = new InfiniteScrollBackground[2];

        public GameObject titlePanel;

        public Intro intro;

        public GameObject eventManager; 
        
        private void Start()
        {
            DataManager.Instance.LoadGameData();
        }
        public void StartGame(bool restart)
        {
            /*isFirstStart = DataManager.Instance.data.introData.isFirstStart;
        
            if (!isFirstStart || restart == true)
            {
                SceneManager.LoadScene("Scenes/IntroScene");
                SaveManager.Instance.WriteIntroData(true);
                DataManager.Instance.SaveGameData();
            }
            else
            {
                SceneManager.LoadScene("Scenes/WorldScene");
            }*/
            foreach (var backGround in backGrounds)
            {
                backGround.needSlowDownScroll = true;
                titlePanel.SetActive(false);
            }
        }
        public void ReStart()
        {
            bool restart = true;
            DataManager.Instance.DeleteGameData();
            DataManager.Instance.LoadGameData();
            StartGame(restart);
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
        public void EndGame()
        {
            Application.Quit();
        }

        private void FixedUpdate()
        {
            if (intro.isPlayed) return;
            
            if (backGrounds[0].startIntro && !intro.isPlayed)
                intro.PlayIntro();
        }

        public void StartGame()
        {
            if (intro.isPlayed) intro.gameObject.SetActive(false); 
            
            Destroy(eventManager);
            
            SceneManager.LoadScene("Scenes/WorldScene");
        } 
    }
}
