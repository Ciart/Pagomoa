using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipitem")]
public class EquipItem : Item
{
    [Header("체력 증가량")]
    public float max_hp_Increase = 0;
    [Space]
    [Header("공격력 증가량")]
    public float attackpower_increase = 0;
    [Space]
    [Header("바깥 시야 증가량(%)")]
    public float out_sight_increase = 0;
    [Header("땅속 시야 증가량(%)")]
    public float in_sight_increase = 0;
    [Header("전체 시야 증가량(%)")]
    public float sight_increase = 0;

    [Space]
    [Header("지상 이동속도 증가량(%)")]
    public float in_speed_increase = 0;
    [Header("지하 이동속도 증가량(%)")]
    public float out_speed_increase = 0;
    [Header("이동속도 증가량(%)")]
    public float move_speed_increase = 0;
    [Space]
    [Header("채굴속도 증가량(%)")]
    public float dig_speed_Increase = 0;
    [Header("기어오르기속도 증가량(%)")]
    public float crawlup_speed_Increase = 0;
    [Space]
    [Header("최대 산소 저장량 증가")]
    public float max_oxygen_Increase = 0;
    [Header("최대 포만감 저장량 증가")]
    public float max_hungry_Increase = 0;
    [Space]
    [Header("체력회복속도 증가량")]
    public float hp_recovery_Increase = 0;
    [Space]
    [Header("산소 회복 증가량(%)")]
    public float oxygen_recovery_Increase = 0;
    [Header("산소 소모 감소량(%)")]
    public float oxygen_consume_decrease = 0;
    [Space]
    [Header("포만감 회복 증가량(%)")]
    public float hungry_recovery_Increase = 0;
    [Header("포만감 소모 감소량(%)")]
    public float hungry_consume_decrease = 0;
}
