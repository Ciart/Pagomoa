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

    [SerializeField] private GameObject[] _monsterType;

    private GameObject _oneByOneMonster;
    
    private int _landMonster;
    
    private GameObject _monsterPrefab;

    private TimeManagerTemp _timeManagerTemp;
    void Start()
    {
        _landMonster = land - 1;
        _monsterPrefab = _monsterType[_landMonster];

        _timeManagerTemp = FindObjectOfType<TimeManagerTemp>().GetComponent<TimeManagerTemp>();
        _timeManagerTemp.NextDaySpawn.AddListener(SpawnMonster);
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
