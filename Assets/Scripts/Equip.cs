using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public List<Item> equipments = new List<Item>();

    public void EquipItem(Item what, bool Calnow = false)
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
        status.sight = basicStatus.sight;

        // Speed Option Init
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
        foreach (Item what in equipments)
        {
            if (!what.Equipable)
                continue;
            what.StatusUpdate(status);
        }
    }


}
