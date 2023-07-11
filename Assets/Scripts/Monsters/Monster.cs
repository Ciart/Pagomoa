using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float hp = 0;
    public float moveSpeed = 0;
    public float damage = 0;
    public int dayNight = 1; // 1 : 낮, 0 : 밤

    private TimeManagerTemp _timeManager;
    private MonsterState _currentState;

    public enum MonsterState
    {
        Active,
        Sleep
    }

    void Start()
    {
        _currentState = MonsterState.Active;
        _timeManager = FindObjectOfType<TimeManagerTemp>().GetComponent<TimeManagerTemp>();
        _timeManager.NextDaySleep.AddListener(SleepAtNight);
    }
    public void SleepAtNight()
    {
        _currentState = MonsterState.Sleep;
        
    }

    public void WakeUp()
    {
        _currentState = MonsterState.Active;
    }
    
    public void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.transform.name == "Player")
        {
            collision2D.transform.GetComponent<PlayerController>().Hit(damage, transform.position);
        }
    }
}
