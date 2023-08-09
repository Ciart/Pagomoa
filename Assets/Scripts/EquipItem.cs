using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipitem")]
public class EquipItem : Itembefore
{
    [Header("���� ������ġ ������ / ����%������ �Դϴ�.")]
    // Armor Option
    [Header("���� ������")]
    public float armorIncrease = 0;
    public float armorIncreasePercentage = 0;
    [Space]

    // Attack Option
    [Header("���ݷ� ������")]
    public float attackpowerIncrease = 0;
    public float attackpowerIncreasePercentage = 0;
    [Space]

    // Sight Option
    [Header("��ü �þ� ������")]
    public float sightIncrease = 0;
    public float sightIncreasePercentage = 0;
    [Header("���� �þ� ������")]
    public float inSightIncrease = 0;
    public float inSightIncreasePercentage = 0;
    [Header("�ٱ� �þ� ������")]
    public float outSightIncrease = 0;
    public float outSightIncreasePercentage = 0;
    [Space]

    // Speed Option
    [Header("�̵��ӵ� ������")]
    public float speedIncrease = 0;
    public float speedIncreasePercentage = 0;
    [Header("���� �̵��ӵ� ������")]
    public float inSpeedIncrease = 0;
    public float inSpeedIncreasePercentage = 0;
    [Header("���� �̵��ӵ� ������")]
    public float outSpeedIncrease = 0;
    public float outSpeedIncreasePercentage = 0;
    [Space]

    // Dig Option
    [Header("ä���ӵ� ������")]
    public float digSpeedIncrease = 0;
    public float digSpeedIncreasePercentage = 0;
    [Space]

    // Crawl Option
    [Header("��������ӵ� ������")]
    public float crawlupSpeedIncrease = 0;
    public float crawlupSpeedIncreasePercentage = 0;
    [Space]

    // Oxygen Option
    [Header("�ִ� ��� ���差 ����")]
    public float maxOxygenIncrease = 0;
    public float maxOxygenIncreasePercentage = 0;
    [Header("��� ȸ�� ������")]
    public float oxygenRecoveryIncrease = 0;
    public float oxygenRecoveryIncreasePercentage = 0;
    [Header("��� �Ҹ� ���ҷ�")]
    public float oxygenConsumeDecrease = 0;
    public float oxygenConsumeDecreasePercentage = 0;
    [Space]

    // Hunngry Option
    [Header("�ִ� ������ ���差 ����")]
    public float maxHungryIncrease = 0;
    public float maxHungryIncreasePercentage = 0;
    [Header("������ ȸ�� ������")]
    public float hungryRecoveryIncrease = 0;
    public float hungryRecoveryIncreasePercentage = 0;
    [Header("������ �Ҹ� ���ҷ�")]
    public float hungryConsumeDecrease = 0;
    public float hungryConsumeDecreasePercentage = 0;


}
