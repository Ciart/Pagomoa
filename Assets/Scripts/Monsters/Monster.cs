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
    
    public MonsterState currentState;
    
    private MonsterManager _monsterManager;
    
    private SpriteRenderer _sleepingAnimation;
    public enum MonsterState
    {
        Active,
        Sleep,
        WakeUpForaWhile
    }

    void Start()
    {
        currentState = MonsterState.Sleep;
        _monsterManager = FindObjectOfType<MonsterManager>().GetComponent<MonsterManager>();
        _sleepingAnimation = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        _monsterManager.MonsterSleep.AddListener(SleepAtNight);
        _monsterManager.MonsterWakeUp.AddListener(WakeUp);
    }
    private void SleepAtNight()
    {
        currentState = MonsterState.Sleep;
        _sleepingAnimation.enabled = true;
    }

    private void WakeUp()
    {
        currentState = MonsterState.Active;
        _sleepingAnimation.enabled = false;
    }
    
    public void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.transform.name == "Player")
        {
            if (currentState == MonsterState.Sleep)
            {
                currentState = MonsterState.WakeUpForaWhile;
            }
            collision2D.transform.GetComponent<PlayerController>().Hit(damage, transform.position);
        }
    }
}
