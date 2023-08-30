using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public bool isFirstStart = false;
    
    public void StartGame(bool restart)
    {
        DataManager.Instance.LoadGameData();
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
        StartGame(restart);
    }
    public void PressStartButton()
    {
        DataManager.Instance.LoadGameData();
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
