using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    private MonsterSpawner _dayTimeSpawner;

    private NightMonsterSpawner _nightSpawner;

    private bool _isSleepTime;
    
    public UnityEvent NextDaySpawn;
    public UnityEvent MonsterSleep;
    public UnityEvent MonsterWakeUp;
    

    void Start()
    {
        _nightSpawner = FindObjectOfType<NightMonsterSpawner>().GetComponent<NightMonsterSpawner>();
    }
    void FixedUpdate()
    {
        if (_isSleepTime)
        { ;
            _nightSpawner.StartNightSpawner();    
        }
    }

    public void NightNoon()
    {
        NextDaySpawn.Invoke();
    }

    public void SleepTime()
    {
        MonsterSleep.Invoke();
        
        _isSleepTime = true;
    }

    public void AwakeTime()
    {
        MonsterWakeUp.Invoke();
        
        _nightSpawner.KillNightMonsters();
        
        _isSleepTime = false;
    }
}
