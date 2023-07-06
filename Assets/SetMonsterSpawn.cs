using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMonsterSpawn : MonoBehaviour
{
    public GameObject MonsterPrefab;
    private int _dayNightType;
    /*
     낮 
     밤
     몬스터 타입
     밤 낮 bool
     낮 몬스터 밤되면 숙면
     시간
     */
    void Start()
    {
        _dayNightType = MonsterPrefab.GetComponent<Monster>().dayNight;
    }

    void Update()
    {
        
    }
    
    
    public void CreateDaytimeMonster()
    {
        
    }

    public void CreateNightMonster()
    {
        
    }
    
    public void EliminateNightMonster()
    {
        
    }
}
