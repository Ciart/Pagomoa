using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Environments;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class TimeManagerTemp : MonoBehaviour
{
    float Minute = 1000;
    float Hour = 60000;


    private int time = 0;
    public int _time { get { return time; } }
    private int maxTime = 1440000; // 24시
    public int _maxTime { get { return maxTime; } }
    

    private int date = 1;
    private float magnification = 1f;

    string season = "";
    bool eventTakePlace = true;

    [HideInInspector] public UnityEvent NextDaySpawn;
    [HideInInspector] public UnityEvent MonsterSleep;
    [HideInInspector] public UnityEvent MonsterWakeUp;
    [HideInInspector] public UnityEvent<FadeState> FadeEvent;

    private static TimeManagerTemp _instance = null;
    public static TimeManagerTemp Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (TimeManagerTemp)FindObjectOfType(typeof(TimeManagerTemp));
            }
            return _instance;
        }
    }
    private void Awake()
    {
        MonsterSleep.AddListener(DayMonster.GetSleep);
        MonsterWakeUp.AddListener(DayMonster.AwakeSleep);
        MonsterWakeUp.AddListener(NightMonster.TimeToBye);
    }
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
        InvokeRepeating(nameof(Timer), magnification, magnification);
    }
    private void Update()
    {
        if (canSleep && Input.GetKeyDown(KeyCode.P))
            Sleep();
    }
    private void FixedUpdate()
    {
        DayLight();
    }
    private void Timer()
    {
        time += 1000;

        if (time >= maxTime)
        {
            time = 0;
            date++;
            NextDaySpawn.Invoke();
        }


        if (season != GetSeasonForPlayer())
        {
            season = GetSeasonForPlayer();
            EventTime();
        }
    }
    private void EventTime()
    {
        if (eventTakePlace == true) return;
        
        if(season == "MiddleNight")
        {
            canSleep = true;
            eventTakePlace = true;
            MonsterSleep.Invoke();
        }
        if (season == "Morning")
        {
            canSleep = false;
            eventTakePlace = true;
            MonsterWakeUp.Invoke();
        }
    }
    public void Sleep()
    {
        FadeEvent.Invoke(FadeState.FadeInOut);
        CancelInvoke(nameof(Timer));
        time = 360000;
        if (time < 24 * Hour && time > 22 * Hour) date++;
        
        InvokeRepeating(nameof(Timer), magnification, magnification);
        
        canSleep = false;
        NextDaySpawn.Invoke();
        MonsterWakeUp.Invoke();
    }
    private void ReturnToBase()
    {
        Vector3 returnPosition = new Vector3(31.7f, 4, 0);
        gameObject.transform.position = returnPosition;
    }
    public string GetSeasonForMonster()
    {
        if (time >= 0 && time < 6 * Hour) // 밤    // 360000
            return "Night";
        else if (time < 22 * Hour) // 낮        // 1320000
            return "Day";
        else
            return "Night";
    }
    public string GetSeasonForPlayer() 
    {
        if (6 * Hour < time && time < 12 * Hour)
            return "Morning";
        else if (time < 18 * Hour)
            return "Afternoon";
        else if (time < 22 * Hour)
            return "Night";
        else
            return "MiddleNight";
    }
    
    void DayLight()
    {
        GetComponent<SkyLightConverter>().Convert(time, maxTime);
    }
}
