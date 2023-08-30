using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManage : MonoBehaviour
{
    public static OptionManage instance = null;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        LoadOption();
    }
    public void LoadOption()
    {
        if(OptionDB.instance)
            gameObject.GetComponent<CanvasScaler>().scaleFactor = OptionDB.instance.scale;
    }
}
