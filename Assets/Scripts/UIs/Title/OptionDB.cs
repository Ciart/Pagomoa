using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDB : MonoBehaviour
{
    public static OptionDB instance = null;

    [SerializeField] public int scale;
    [SerializeField] public float audioValue;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
                Destroy(gameObject);
        }
        SaveManager.Instance.LoadOption();
        GameObject.Find("Canvas").GetComponent<CanvasScaler>().scaleFactor = scale;
    }
}
