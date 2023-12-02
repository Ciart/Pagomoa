using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Worlds;

public class Logger : MonoBehaviour
{

    [Header("TimeManager")] [Space]
    public int date;
    public int hour;
    public int time;
    [Header("Collect")] [Space] 
    public int mineral;
    // 파워 스톤 
    // 고유 아이템 처리
    [Header("WorldManager")] [Space] 
    public int brick;
    
    
    # region 인스턴스 
    private static Logger _instance;
    public static Logger Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (Logger)FindObjectOfType(typeof(Logger));
            }
            return _instance;
        }
    }
    #endregion
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(date);
            Debug.Log(time);
            Debug.Log(mineral);
            Debug.Log(brick);
        }
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(UpdateRealTime), 0, 60);
    }
    
    public bool LoggingObject<T>(T arg)
    {
        switch (arg)
        {
            case TimeManager:
                LoggingDate();
                return true;
            case Collect:
                LoggingMineral();
                return true;
            case WorldManager:
                LoggingBrick();
                return true;
            default:
                return false;
                break;
        }
    }

    #region TimeManager
    private void UpdateRealTime() { time++; }
    
    private void LoggingDate() { date++; }
    #endregion


    private void LoggingBrick() { brick++; }

    private void LoggingMineral() { mineral++; }
}
