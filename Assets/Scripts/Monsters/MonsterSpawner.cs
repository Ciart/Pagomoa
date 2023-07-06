using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int land;

    public GameObject[] monsterType;
    
    private int _landMonster;
    
    private Monster _monster;

    private TimeManager _timeManager;
    
    private bool _isNextDay = false;

    private GameObject _monsterPrefab;

    private GameObject _oneByOneMonster;
    void Start()
    {
        _landMonster = land - 1;
        _monsterPrefab = monsterType[_landMonster];

        _monster = _monsterPrefab.GetComponent<Monster>();
        _timeManager = GameObject.FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
    }
    void Update()
    {
        SpawnMonster();
    }

    public void SpawnMonster()
    {
        _isNextDay = _timeManager.NextDay();
        if (_isNextDay)
        {
            CheckIsMonster();
        }
    }
    
    public void SleepAtNight()
    {
        
    }

    private void CheckIsMonster()
    {
        if (_oneByOneMonster)
        {
            return ;
        } else {
            _oneByOneMonster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
        }
    }
}
