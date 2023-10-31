using Quest;
using UnityEngine;
using UnityEngine.Events;

public class TimeManagerTemp : MonoBehaviour
{
    private int time = 0;
    public int _time { get { return time; } }
    private int maxTime = 1440000;
    public int _maxTime { get { return maxTime; } }
    private int startTime = 360000;
    private int endTime = 1320000; // 22시 ~ 06시 
    private int returnTime = 60000;
    private int _wakeUpTime = 360000;
    private int date = 1;
    private float magnification = 0.10f;

    public UnityEvent NextDaySpawn;
    public UnityEvent MonsterSleep;
    public UnityEvent MonsterWakeUp;
    public UnityEvent<FadeState> FadeEvent;

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
        InvokeRepeating(nameof(StartTime), magnification, magnification);
    }
    private void Update()
    {
        if (canSleep && Input.GetKeyDown(KeyCode.P))
            Sleep();

        //if (Input.GetKeyDown(KeyCode.C))
        //    time = endTime - 5000;

        //if (Input.GetKeyDown(KeyCode.V))
        //    time = _wakeUpTime - 5000;

        if (Input.GetKeyDown(KeyCode.B))
        {
            time += 60000;
            Debug.Log(_hour + "시 ");
        }


    }
    private void FixedUpdate()
    {
        DayLight();
    }
    private void StartTime()
    {
        // too noise
        //Debug.Log(date +"일차 " + _hour + "시 " + _minute + "분");
        //Debug.Log(_hour + "시 ");

        time += 100000;
        EventTime();
    }
    private void EventTime()
    {
        if (time >= maxTime) // 날짜 바뀌는 시간
        {
            time = 0;
            date++;
            NextDaySpawn.Invoke();
            Debug.Log("다음 날이야!");
            
            GameLogger.Instance.LogObject(GameLogger.LoggingGeneral.Date);
            Debug.Log(GameLogger.Instance.GetObjectCount(GameLogger.LoggingGeneral.Date));
            Debug.Log(GameLogger.Instance.GetObjectCount(GameLogger.LoggingGeneral.Brick));
        }
        if (time == endTime) // 잠자는 시간 22 ~ 06
        {
            canSleep = true;
            MonsterSleep.Invoke();
            Debug.Log(_hour + "시 " + _minute + "분" + " 잘자렴!");

        }
        if (time == returnTime)
        {
            ReturnToBase();
        }
        if (time == _wakeUpTime)
        {
            canSleep = false;
            MonsterWakeUp.Invoke();
            Debug.Log(_hour + "시 " + _minute + "분" + " 일어나!");

        }
    }
    public void Sleep()
    {
        FadeEvent.Invoke(FadeState.FadeInOut);
        CancelInvoke(nameof(StartTime));
        time = startTime;
        if (time < 1440000 && time > 1320000) date++;
        
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
    public string GetSeasonForMonster()
    {
        if (time >= 0 && time < _wakeUpTime) // 밤    // 360000
            return "Night";
        else if (time < endTime) // 낮        // 1320000
            return "Day";
        else
            return "Night";
    }
    
    void DayLight()
    {
        GetComponent<EnvironmentConverter>().Convert(time, maxTime);
    }
}