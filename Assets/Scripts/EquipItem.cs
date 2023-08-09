using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipitem")]
public class EquipItem : Itembefore
{
    [Header("각각 고정수치 증가량 / 비율%증가량 입니다.")]
    // Armor Option
    [Header("방어력 증가량")]
    public float armorIncrease = 0;
    public float armorIncreasePercentage = 0;
    [Space]

    // Attack Option
    [Header("공격력 증가량")]
    public float attackpowerIncrease = 0;
    public float attackpowerIncreasePercentage = 0;
    [Space]

    // Sight Option
    [Header("전체 시야 증가량")]
    public float sightIncrease = 0;
    public float sightIncreasePercentage = 0;
    [Header("땅속 시야 증가량")]
    public float inSightIncrease = 0;
    public float inSightIncreasePercentage = 0;
    [Header("바깥 시야 증가량")]
    public float outSightIncrease = 0;
    public float outSightIncreasePercentage = 0;
    [Space]

    // Speed Option
    [Header("이동속도 증가량")]
    public float speedIncrease = 0;
    public float speedIncreasePercentage = 0;
    [Header("지상 이동속도 증가량")]
    public float inSpeedIncrease = 0;
    public float inSpeedIncreasePercentage = 0;
    [Header("지하 이동속도 증가량")]
    public float outSpeedIncrease = 0;
    public float outSpeedIncreasePercentage = 0;
    [Space]

    // Dig Option
    [Header("채굴속도 증가량")]
    public float digSpeedIncrease = 0;
    public float digSpeedIncreasePercentage = 0;
    [Space]

    // Crawl Option
    [Header("기어오르기속도 증가량")]
    public float crawlupSpeedIncrease = 0;
    public float crawlupSpeedIncreasePercentage = 0;
    [Space]

    // Oxygen Option
    [Header("최대 산소 저장량 증가")]
    public float maxOxygenIncrease = 0;
    public float maxOxygenIncreasePercentage = 0;
    [Header("산소 회복 증가량")]
    public float oxygenRecoveryIncrease = 0;
    public float oxygenRecoveryIncreasePercentage = 0;
    [Header("산소 소모 감소량")]
    public float oxygenConsumeDecrease = 0;
    public float oxygenConsumeDecreasePercentage = 0;
    [Space]

    // Hunngry Option
    [Header("최대 포만감 저장량 증가")]
    public float maxHungryIncrease = 0;
    public float maxHungryIncreasePercentage = 0;
    [Header("포만감 회복 증가량")]
    public float hungryRecoveryIncrease = 0;
    public float hungryRecoveryIncreasePercentage = 0;
    [Header("포만감 소모 감소량")]
    public float hungryConsumeDecrease = 0;
    public float hungryConsumeDecreasePercentage = 0;


}
