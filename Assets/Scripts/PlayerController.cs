using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Status))]
public class PlayerController : MonoBehaviour
{
    public Status status;
    public Status basicStatus;

    public float GroundHeight = 0;

    public LayerMask WhatisGround;

    public float oxygen_consume = 0.001f;

    public floatfloatEvent oxygen_alter;
    public floatfloatEvent hungry_alter;
    public floatEvent direction_alter;

    public List<EquipItem> equipments = new List<EquipItem>();

    [System.Serializable]
    public class floatfloatEvent : UnityEvent<float, float> { }
    [System.Serializable]
    public class floatEvent : UnityEvent<float> { }
    // Update is called once per frame
    private void Awake()
    {
        basicStatus = status.copy();
        CalEquipvalue();       
        if (oxygen_alter == null)
            oxygen_alter = new floatfloatEvent();
        if (hungry_alter == null)
            hungry_alter = new floatfloatEvent();
        if (direction_alter == null)
            direction_alter = new floatEvent();

        Debug.Log(status.maxhp);
        Debug.Log(GetComponent<Status>().maxhp);

    }
    void FixedUpdate()
    { 
        if (transform.position.y < GroundHeight && status.oxygen >= status.min_oxygen)
            status.oxygen -= Mathf.Abs(transform.position.y) * oxygen_consume;
        else
            if(status.oxygen < status.max_oxygen)
                status.oxygen += Mathf.Abs(transform.position.y) * oxygen_consume;
        oxygen_alter.Invoke(status.oxygen, status.max_oxygen);
    }
    public void SetDirection(float direction)
    {
        if (direction != 0)
        {
            Vector3 Direction = new Vector3(direction, 1, 1);
            transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z) ;
            direction_alter.Invoke(direction);
        }
    }
    public void Equip(EquipItem what, bool Calnow = false)
    {
        equipments.Add(what);
        if(Calnow)
            CalEquipvalue();
    }

    public void CalEquipvalue()
    {
        status.maxhp = basicStatus.maxhp;

        status.attackpower = basicStatus.attackpower;

        status.outsight = basicStatus.outsight;
        status.insight = basicStatus.insight;
        status.sight = basicStatus.sight;

        status.outspeed = basicStatus.outspeed;
        status.inspeed = basicStatus.inspeed;
        status.speed = basicStatus.speed;

        status.dig_speed = basicStatus.dig_speed;
        status.crawlup_speed = basicStatus.crawlup_speed;

        status.max_oxygen = basicStatus.max_oxygen;
        status.max_hungry = basicStatus.max_hungry;

        status.hp_recovery = basicStatus.hp_recovery;

        status.oxygen_recovery = basicStatus.oxygen_recovery;
        status.oxygen_consume = basicStatus.oxygen_consume;

        status.hungry_recovery = basicStatus.hungry_recovery;
        status.hungry_consume = basicStatus.hungry_consume;



        float total_maxhp_increase = 0;

        float total_attackpower_increase = 0;

        float total_outsight_increase = 0;
        float total_insight_increase = 0;
        float total_sight_increase = 0;

        float total_outspeed_increase = 0;
        float total_inspeed_increase = 0;
        float total_movespeed_increase = 0;

        float total_dig_speed_increase = 0;
        float total_crawlup_speed_increase = 0;

        float total_max_oxygen_increase = 0;
        float total_max_hungry_increase = 0;

        float total_hp_recovery_increase = 0;

        float total_oxygen_recovery_increase = 0;
        float total_oxygen_consume_decrease = 0;

        float total_hungry_recovery_increase = 0;
        float total_hungry_consume_decrease = 0;

        foreach (EquipItem what in equipments)
        {
            total_maxhp_increase += what.max_hp_Increase;

            total_attackpower_increase += what.attackpower_increase;

            total_outsight_increase += what.out_sight_increase;
            total_insight_increase += what.in_sight_increase;
            total_sight_increase += what.sight_increase;
            total_outsight_increase += what.sight_increase;
            total_insight_increase += what.sight_increase;



            total_outspeed_increase += what.out_speed_increase;
            total_inspeed_increase += what.in_speed_increase;
            total_movespeed_increase += what.move_speed_increase;
            total_outspeed_increase += what.move_speed_increase;
            total_inspeed_increase += what.move_speed_increase;

            total_dig_speed_increase += what.dig_speed_Increase;
            total_crawlup_speed_increase += what.crawlup_speed_Increase;

            total_max_oxygen_increase += what.max_oxygen_Increase;
            total_max_hungry_increase += what.max_hungry_Increase;

            total_hp_recovery_increase += what.hp_recovery_Increase;

            total_oxygen_recovery_increase += what.oxygen_recovery_Increase;
            total_oxygen_consume_decrease += what.oxygen_consume_decrease;

            total_hungry_recovery_increase += what.hungry_recovery_Increase;
            total_hungry_consume_decrease += what.hungry_consume_decrease;
        }


        status.maxhp += total_maxhp_increase;

        status.attackpower += total_attackpower_increase;

        status.outsight *= (1 + (total_outsight_increase / 100f));
        status.insight *= (1 + (total_insight_increase / 100f));
        status.sight *= (1 + (total_sight_increase / 100f));

        status.outspeed *= (1 + total_outspeed_increase / 100f);
        status.inspeed *= (1 + total_inspeed_increase / 100f);
        status.speed *= (1 + total_movespeed_increase / 100f);

        status.dig_speed *= (1 + total_dig_speed_increase / 100f);
        status.crawlup_speed *= (1 + total_crawlup_speed_increase / 100f);

        status.max_oxygen += total_max_oxygen_increase;
        status.max_hungry += total_max_hungry_increase;

        status.hp_recovery += total_hp_recovery_increase;

        status.oxygen_recovery *= (1 + total_oxygen_recovery_increase / 100f);
        status.oxygen_consume *= (1 + total_oxygen_consume_decrease / 100f);

        status.hungry_recovery *= (1 + total_hungry_recovery_increase / 100f);
        status.hungry_consume *= (1 + total_hungry_consume_decrease / 100f);

    }
}
