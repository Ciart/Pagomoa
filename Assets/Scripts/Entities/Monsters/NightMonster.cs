using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMonster : Monster
{
    private void Awake()
    {

        _animator = GetComponent<Animator>();
        status = GetComponent<MonsterStatus>();
        _attack = GetComponent<Attack>();

        _controller = GetComponent<MonsterController>();
    }
    public static void TimeToBye()
    {
        NightMonster[] Monsters = GameObject.FindObjectsOfType<NightMonster>();
        foreach (NightMonster monster in Monsters)
            monster.Dissapear();
    }
    public void Dissapear()
    {
        _controller.StateChanged(MonsterState.Die);
    }
    
}
