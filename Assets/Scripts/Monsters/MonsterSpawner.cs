using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    
    public int land;

    public GameObject[] monsterType;

    private GameObject _oneByOneMonster;
    
    private int _landMonster;
    
    private GameObject _monsterPrefab;

    public TimeManagerTemp _timeManager;
    
    void Start()
    {
        _landMonster = land - 1;
        _monsterPrefab = monsterType[_landMonster];

        _timeManager = FindObjectOfType<TimeManagerTemp>().GetComponent<TimeManagerTemp>();
        _timeManager.NextDaySpawn.AddListener(SpawnMonster);
    }

    public void SpawnMonster()
    {
        CheckIsMonster();
    }

    private void CheckIsMonster()
    {
        if (_oneByOneMonster)
        {
            return ;
        }
        else if (!_oneByOneMonster)
        {
            _oneByOneMonster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
        }
    }
    
    /*private void OnDestroy()
    {
        Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
    }*/
}
