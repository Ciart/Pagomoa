using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private AchieveContents achieveContents;
    [SerializeField] private int time = 0;
    private int startTime = 360000;
    private int endTime = 1320000;
    private int returnTime = 60000;
    private int date = 1;
    private float magnification = 1.0f;

    private static TimeManager _instance = null;
    public static TimeManager Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (TimeManager)FindObjectOfType(typeof(TimeManager));
            }
            return _instance;
        }
    }

    private int hour
    {
        get { return time / 60000; }
    }
    private int minute
    {
        get { return time % 60000 / 1000; }
    }
    private bool canSleep = false;

    private void Start()
    {
        InvokeRepeating("StartTime", magnification, magnification);
    }
    private void Update()
    {
        if (canSleep == true && Input.GetKeyDown(KeyCode.P))
            Sleep();
    }
    private void StartTime()
    {
        //Debug.Log(date +"���� " + hour + "�� " + minute + "��");
        time += 1000;
        EventTime();
    }
    private void EventTime()
    {
        if (time == 1440000) // ��¥ �ٲ�� �ð�
        {
            time = 0;
            date++;
        }
        else if (time == endTime) // ���ڴ� �ð�
        {
            canSleep = true;
            Debug.Log("�� �� �ִ�.");
        }
        else if (time == returnTime)
        {
            ReturnToBase();
        }
    }
    private void Sleep()
    {
        CancelInvoke("StartTime");
        Debug.Log("�帣��");
        time = startTime;
        date++;
        //
        Logger.Instance.LoggingObject(this);
        //
        achieveContents.ChangePrice();
        InvokeRepeating("StartTime", magnification, magnification);
    }
    private void ReturnToBase()
    {
        Vector3 returnPosition = new Vector3(31.7f, 4, 0);
        gameObject.transform.position = returnPosition;
    }
}
