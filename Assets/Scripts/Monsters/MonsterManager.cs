using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    private NightMonsterSpawner _nightSpawner;
    
    private TimeManagerTemp _timeManagerTemp;

    private bool _isSleepTime;

    void Start()
    {
        _nightSpawner = FindObjectOfType<NightMonsterSpawner>().GetComponent<NightMonsterSpawner>();
        _timeManagerTemp = FindObjectOfType<TimeManagerTemp>().GetComponent<TimeManagerTemp>();
        
        _timeManagerTemp.MonsterSleep.AddListener(SleepTime);
        _timeManagerTemp.MonsterWakeUp.AddListener(AwakeTime);
    }
    void FixedUpdate()
    {
        if (_isSleepTime)
        { 
            _nightSpawner.StartNightSpawner();    
        }
    }

    public void SleepTime()
    {
        _isSleepTime = true;
    }

    public void AwakeTime()
    {
        _nightSpawner.KillNightMonsters();
        _isSleepTime = false;
    }
}
