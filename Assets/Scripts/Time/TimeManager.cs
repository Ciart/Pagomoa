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
        //Debug.Log(date +"일차 " + hour + "시 " + minute + "분");
        time += 1000;
        EventTime();
    }
    private void EventTime()
    {
        if (time == 1440000) // 날짜 바뀌는 시간
        {
            time = 0;
            date++;
        }
        else if (time == endTime) // 잠자는 시간
        {
            canSleep = true;
            Debug.Log("잘 수 있다.");
        }
        else if (time == returnTime)
        {
            ReturnToBase();
        }
    }
    private void Sleep()
    {
        CancelInvoke("StartTime");
        Debug.Log("드르렁");
        time = startTime;
        date++;
        achieveContents.ChangePrice();
        InvokeRepeating("StartTime", magnification, magnification);
    }
    private void ReturnToBase()
    {
        Vector3 returnPosition = new Vector3(31.7f, 4, 0);
        gameObject.transform.position = returnPosition;
    }
}
