using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    private NightMonsterSpawner _nightSpawner;
    
    private TimeManager _timeManager;

    private bool _isSleepTime;

    void Start()
    {
        _nightSpawner = FindObjectOfType<NightMonsterSpawner>().GetComponent<NightMonsterSpawner>();
        _timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
        
        _timeManager.MonsterSleep.AddListener(SleepTime);
        _timeManager.MonsterWakeUp.AddListener(AwakeTime);
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
