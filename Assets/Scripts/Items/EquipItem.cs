using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipitem")]
public class EquipItem : Item
{
    [Header("ü�� ������")]
    public float max_hp_Increase = 0;
    [Space]
    [Header("���ݷ� ������")]
    public float attackpower_increase = 0;
    [Space]
    [Header("�ٱ� �þ� ������(%)")]
    public float out_sight_increase = 0;
    [Header("���� �þ� ������(%)")]
    public float in_sight_increase = 0;
    [Header("��ü �þ� ������(%)")]
    public float sight_increase = 0;

    [Space]
    [Header("���� �̵��ӵ� ������(%)")]
    public float in_speed_increase = 0;
    [Header("���� �̵��ӵ� ������(%)")]
    public float out_speed_increase = 0;
    [Header("�̵��ӵ� ������(%)")]
    public float move_speed_increase = 0;
    [Space]
    [Header("ä���ӵ� ������(%)")]
    public float dig_speed_Increase = 0;
    [Header("��������ӵ� ������(%)")]
    public float crawlup_speed_Increase = 0;
    [Space]
    [Header("�ִ� ��� ���差 ����")]
    public float max_oxygen_Increase = 0;
    [Header("�ִ� ������ ���差 ����")]
    public float max_hungry_Increase = 0;
    [Space]
    [Header("ü��ȸ���ӵ� ������")]
    public float hp_recovery_Increase = 0;
    [Space]
    [Header("��� ȸ�� ������(%)")]
    public float oxygen_recovery_Increase = 0;
    [Header("��� �Ҹ� ���ҷ�(%)")]
    public float oxygen_consume_decrease = 0;
    [Space]
    [Header("������ ȸ�� ������(%)")]
    public float hungry_recovery_Increase = 0;
    [Header("������ �Ҹ� ���ҷ�(%)")]
    public float hungry_consume_decrease = 0;
}
