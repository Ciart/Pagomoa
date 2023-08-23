using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public List<EquipItem> equipments = new List<EquipItem>();

    private void Awake()
    {
        //equipments 
    }

    public void EquipItem(EquipItem what, bool Calnow = false)
    {
        equipments.Add(what);
        if (Calnow)
            CalEquipvalue();
    }

    public void CalEquipvalue()
    {
        Player.Status status = GetComponent<Player.PlayerController>()._status;
        Player.Status basicStatus = GetComponent<Player.PlayerController>()._initialStatus;

        // Hp Option Init

        // Armor Option Init
        status.armor = basicStatus.armor;

        // AttackPower Option Init
        status.attackpower = basicStatus.attackpower;

        // SIght Option Init
        status.outSight = basicStatus.outSight;
        status.inSight = basicStatus.inSight;
        status.sight = basicStatus.sight;

        // Speed Option Init
        status.outSpeed = basicStatus.outSpeed;
        status.inSpeed = basicStatus.inSpeed;
        status.speed = basicStatus.speed;

        // Dig Option Init
        status.digSpeed = basicStatus.digSpeed;

        // Crawlup Option Init
        status.crawlUpSpeed = basicStatus.crawlUpSpeed;

        // Oxygen Option Init
        status.maxOxygen = basicStatus.maxOxygen;
        status.oxygenRecovery = basicStatus.oxygenRecovery;
        status.oxygenConsume = basicStatus.oxygenConsume;

        // Hungry Option Init
        status.maxHungry = basicStatus.maxHungry;
        status.hungryRecovery = basicStatus.hungryRecovery;
        status.hungryConsume = basicStatus.hungryConsume;


        //////////////////////////////////////////



        // Hp(Extra) Option Init
        //status.extraHp = 0;
        //status.extraHpPercentage = 0;

        //status.extraHpRecovery = 0;
        //status.extraHpRecoveryPercentage = 0;

        // Armor Option Init
        status.extraArmor = 0;
        status.extraArmorPercentage = 0;

        // AttackPower(Extra) Option Init
        status.extraAttackpower = 0;
        status.extraAttackpowerPercentage = 0;

        // SIght Option(Extra) Init
        status.extraSIght = 0;
        status.extraSIghtPercentage = 0;

        status.extraInSIght = 0;
        status.extraInSIghtPercentage = 0;

        status.extraOutSight = 0;
        status.extraOutSightPercentage = 0;

        // Speed Option(Extra) Init
        status.extraSpeed = 0;
        status.extraSpeedPercentage = 0;

        status.extraInSpeed = 0;
        status.extraInSpeedPercentage = 0;

        status.extraOutSpeed = 0;
        status.extraOutSpeedPercentage = 0;

        // Dig(Extra) Option Init
        status.extraDigSpeed = 0;
        status.extraDigSpeedPercentage = 0;

        // Crawlup(Extra) Option Init
        status.extraCrawlUpSpeed = 0;
        status.extraCrawlUpSpeedPercentage = 0;

        // Oxygen(Extra) Option Init
        status.extramaxOxygen = 0;
        status.extraMaxOxygenPercentage = 0;

        status.extraOxygenRecovery = 0;
        status.extraOxygenRecoveryPercentage = 0;

        status.extraOxygenConsume = 0;
        status.extraOxygenConsumePercentage = 0;


        // Hungry(Extra) Option Init
        status.extraHungry = 0;
        status.extraHungryPercentage = 0;

        status.extraHungryRecovery = 0;
        status.extraHungryRecoveryPercentage = 0;

        status.extraHungryConsume = 0;
        status.extraHungryConsumePercentage = 0;
        foreach (EquipItem what in equipments)
        {
            //// Hp(extra) increase
            //status.extraHp += what.maxHpIncrease;
            //status.extraHpPercentage += what.maxHpIncreasePercentage;

            //status.extraHpRecovery += what.hpRecoveryIncrease;
            //status.extraHpRecoveryPercentage += what.hpRecoveryIncreasePercentage;

            // Armor(extra) increase
            status.extraArmor += what.armorIncrease;
            status.extraArmorPercentage += what.armorIncreasePercentage;

            // AttackPower(extra) increase
            status.extraAttackpower += what.attackpowerIncrease;
            status.extraAttackpowerPercentage += what.attackpowerIncreasePercentage;

            // Sight(extra) increase
            status.extraSIght += what.sightIncrease;
            status.extraSIghtPercentage += what.sightIncreasePercentage;

            status.extraInSIght += what.inSightIncrease;
            status.extraInSIghtPercentage += what.inSightIncreasePercentage;

            status.extraOutSight += what.outSightIncrease;
            status.extraOutSightPercentage += what.outSightIncreasePercentage;

            // Speed(extra) increase
            status.extraSpeed += what.speedIncrease;
            status.extraSpeedPercentage += what.speedIncreasePercentage;

            status.extraInSpeed += what.inSpeedIncrease;
            status.extraInSpeedPercentage += what.inSpeedIncreasePercentage;

            status.extraOutSpeed += what.outSpeedIncrease;
            status.extraOutSpeedPercentage += what.outSpeedIncreasePercentage;

            // Dig(extra) increase
            status.extraDigSpeed += what.digSpeedIncrease;
            status.extraDigSpeedPercentage += what.digSpeedIncreasePercentage;

            // Crawl(extra) increase
            status.extraCrawlUpSpeed += what.crawlupSpeedIncrease;
            status.extraCrawlUpSpeedPercentage += what.crawlupSpeedIncreasePercentage;

            // Oxygen(extra) increase
            status.extramaxOxygen += what.maxOxygenIncrease;
            status.extraMaxOxygenPercentage += what.maxOxygenIncreasePercentage;

            status.extraOxygenRecovery += what.oxygenRecoveryIncrease;
            status.extraOxygenRecoveryPercentage += what.oxygenRecoveryIncreasePercentage;

            status.extraOxygenConsume += what.oxygenConsumeDecrease;
            status.extraOxygenConsumePercentage += what.oxygenConsumeDecreasePercentage;

            // Hungry(extra) increase
            status.extraHungry += what.maxHungryIncrease;
            status.extraHungryConsumePercentage += what.maxHungryIncreasePercentage;

            status.extraHungryRecovery += what.hungryRecoveryIncrease;
            status.extraHungryRecoveryPercentage += what.hungryRecoveryIncreasePercentage;

            status.extraHungryConsume += what.hungryConsumeDecrease;
            status.extraHungryConsumePercentage += what.hungryConsumeDecreasePercentage;

        }

        // Hp Apply Formula
        //status.maxHp *= 1 + (status.extraHpPercentage / 100f);
        //status.maxHp += status.extraHp;

        //status.hpRecovery *= 1 + (status.extraHpRecoveryPercentage / 100f);
        //status.hpRecovery += status.extraHpRecovery;

        // Armor
        status.armor *= 1 + (status.extraArmorPercentage / 100f);
        status.armor += status.extraArmor;
        // AttackPower Apply Fomula
        status.attackpower *= 1 + (status.extraAttackpowerPercentage / 100f);
        status.attackpower += status.extraAttackpower;

        // Sight Apply Formula
        status.sight *= 1 + (status.extraSIghtPercentage / 100f);
        status.sight += status.extraSIght;

        status.inSight *= 1 + (status.extraInSIghtPercentage + status.extraSIghtPercentage / 100f);
        status.inSight += status.extraInSIght + status.extraSIght;

        status.outSight *= 1 + ((status.extraOutSightPercentage + status.extraSIghtPercentage) / 100f);
        status.outSight += status.extraOutSight + status.extraSIght;

        // Speed Apply Formula
        status.speed *= 1 + (status.extraSpeedPercentage / 100f);
        status.speed += status.extraSpeed;

        status.inSpeed *= 1 + ((status.extraInSpeedPercentage + status.extraSpeedPercentage) / 100f);
        status.inSpeed += status.extraInSpeed + status.extraSpeed;

        status.outSpeed *= 1 + ((status.extraOutSpeedPercentage + status.extraSpeedPercentage) / 100f);
        status.outSpeed += status.extraOutSpeed + status.extraSpeed;

        // Dig Apply Formula
        status.digSpeed *= 1 + (status.extraDigSpeedPercentage / 100f);
        status.digSpeed += status.extraDigSpeed;

        // Dig Crawl Formula
        status.crawlUpSpeed *= 1 + (status.extraCrawlUpSpeedPercentage / 100f);
        status.crawlUpSpeed += status.extraCrawlUpSpeed;

        // Oxygen Formula
        status.maxOxygen *= 1 + (status.extraMaxOxygenPercentage / 100f);
        status.maxOxygen += status.extramaxOxygen;

        status.oxygenRecovery *= 1 + (status.extraOxygenRecoveryPercentage / 100f);
        status.oxygenRecovery += status.extraOxygenRecovery;

        status.oxygenConsume *= 1 + (status.extraOxygenConsume / 100f);
        status.oxygenConsume += status.extraOxygenConsume;

        // Hungry Formula
        status.maxHungry *= 1 + (status.extraHungryPercentage / 100f);
        status.maxHungry += status.extraHungry;

        status.hungryRecovery *= 1 + (status.extraHungryRecoveryPercentage / 100f);
        status.hungryRecovery += status.extraHungryRecovery;

        status.hungryConsume *= 1 + (status.extraHungryConsumePercentage / 100f);
        status.hungryConsume += status.extraHungryConsume;

    }


}
