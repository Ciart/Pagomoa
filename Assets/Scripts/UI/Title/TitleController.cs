using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.UI.Title
{
    public class TitleController : MonoBehaviour
    {
        public bool isFirstStart = false;

        public ScrollBackground[] backGrounds = new ScrollBackground[3];

        public GameObject titlePanel;
        
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
                backGround.startIntro = true;
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
        
    }
}
