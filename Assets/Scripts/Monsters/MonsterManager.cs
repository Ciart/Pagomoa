using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private MonsterSpawner _spawner;

    private TimeManagerTemp _timeManagerTemp;
    
    
    void Start()
    {
        _timeManagerTemp = FindObjectOfType<TimeManagerTemp>().GetComponent<TimeManagerTemp>();
    }
    void Update()
    {
        
    }
}
