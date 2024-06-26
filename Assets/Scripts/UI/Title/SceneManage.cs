using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa.UI.Title
{
    public class SceneManage : MonoBehaviour
    {
        public bool isFirstStart = false;

        private void Start()
        {
            DataManager.Instance.LoadGameData();
        }
        public void StartGame(bool restart)
        {
            isFirstStart = DataManager.Instance.data.introData.isFirstStart;
        
            if (!isFirstStart || restart == true)
            {
                SceneManager.LoadScene("Scenes/IntroScene");
                SaveManager.Instance.WriteIntroData(true);
                DataManager.Instance.SaveGameData();
            }
            else
            {
                SceneManager.LoadScene("Scenes/WorldScene");
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
