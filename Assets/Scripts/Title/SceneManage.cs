using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public bool isFirstStart = false;
    
    public void StartGame()
    {
        DataManager.Instance.LoadGameData();
        isFirstStart = DataManager.Instance.data.introData.isFirstStart;
        
        if (!isFirstStart)
        {
            SceneManager.LoadScene("Scenes/WorldScene");
            //SceneManager.LoadScene("Scenes/IntroScene");
            SaveManager.Instance.WriteIntroData(true);
            DataManager.Instance.SaveGameData();
        }
        else
        {
            SceneManager.LoadScene("Scenes/WorldScene");
        }
    }
    public void EndGame()
    {
        Application.Quit();
    }

}
