using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TimeManagerTemp : MonoBehaviour
{
    [SerializeField] private int time = 0;
    private int startTime = 360000;
    private int endTime = 1320000; // 22시 ~ 06시 
    private int returnTime = 60000;
    private int _wakeUpTime = 360000;
    private int date = 1;
    private float magnification = 1.0f;

    public UnityEvent NextDaySpawn;
    public UnityEvent MonsterSleep;
    public UnityEvent MonsterWakeUp;
    public UnityEvent<FadeState> FadeEvent;

    private int _hour
    {
        get { return time / 60000; }
    }
    private int _minute
    {
        get { return time % 60000 / 1000; }
    }
    public bool canSleep = false;
    
    private void Start()
    {
        InvokeRepeating(nameof(StartTime), magnification, magnification);
    }
    
    private void StartTime()
    {
        Debug.Log(date +"일차 " + _hour + "시 " + _minute + "분");
        time += 1000;
        EventTime();
    }
    private void EventTime()
    {
        if (time == 1440000) // 날짜 바뀌는 시간
        {
            time = 0;
            date++;
            NextDaySpawn.Invoke();
        }
        if (time == endTime) // 잠자는 시간 22 ~ 06
        {
            canSleep = true;
            MonsterSleep.Invoke();
        }
        if (time == returnTime)
        {
            ReturnToBase();
        }
        if (time == _wakeUpTime)
        {
            canSleep = false;
            MonsterWakeUp.Invoke();
        }
    }
    public void Sleep()
    {
        FadeEvent.Invoke(FadeState.FadeInOut);
        CancelInvoke(nameof(StartTime));
        time = startTime;
        date++;
        
        InvokeRepeating(nameof(StartTime), magnification, magnification);
        
        canSleep = false;
        NextDaySpawn.Invoke();
        MonsterWakeUp.Invoke();
    }
    private void ReturnToBase()
    {
        Vector3 returnPosition = new Vector3(31.7f, 4, 0);
        gameObject.transform.position = returnPosition;
    }
}