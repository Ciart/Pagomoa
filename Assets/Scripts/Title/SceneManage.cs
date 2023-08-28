using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void StartScene()
    {
        
        SceneManager.LoadScene("Scenes/WorldScene");
    }
    public void EndGame()
    {
        Application.Quit();
    }

    public void StartIntroScene()
    {
        SceneManager.LoadScene("Scenes/IntroScene");
    }
    
}
