using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
