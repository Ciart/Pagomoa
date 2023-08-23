using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equip item")]
public class EquipableItem : Item
{
    public bool Equipable;

    public DicList<string, float> AbilityList;

    public override void Active(Status stat)
    {
        Equip(stat);
    }
    public virtual void Equip(Status status)
    {
        Dictionary<string, float> keyValuePairs;
        keyValuePairs = ListDictionaryConverter.ToDictionary(AbilityList);
        foreach (string key in keyValuePairs.Keys)
        {
            switch (key)
            {
                case "oxygen+":
                    status.maxOxygen += keyValuePairs[key];
                    break;
                case "oxygen%":
                    status.maxOxygen *= keyValuePairs[key];
                    break;
                case "oxygenRecovery+":
                    status.oxygenRecovery += keyValuePairs[key];
                    break;
                case "oxygenRecovery%":
                    status.oxygenRecovery *= keyValuePairs[key];
                    break;
                case "oxygenConsume+":
                    status.oxygenConsume += keyValuePairs[key];
                    break;
                case "oxygenConsume%":
                    status.oxygenConsume *= keyValuePairs[key];
                    break;
                case "hungry+":
                    status.hungry += keyValuePairs[key];
                    break;
                case "hungry%":
                    status.hungry *= keyValuePairs[key];
                    break;
                case "hungryRecovery+":
                    status.hungryRecovery += keyValuePairs[key];
                    break;
                case "hungryRecovery%":
                    status.hungryRecovery *= keyValuePairs[key];
                    break;
                case "hungryConsume+":
                    status.hungryConsume += keyValuePairs[key];
                    break;
                case "hungryConsume%":
                    status.hungryConsume *= keyValuePairs[key];
                    break;
                case "armor+":
                    status.armor += keyValuePairs[key];
                    break;
                case "armor%":
                    status.armor *= keyValuePairs[key];
                    break;
                case "attackPower+":
                    status.attackpower += keyValuePairs[key];
                    break;
                case "attackPower%":
                    status.attackpower *= keyValuePairs[key];
                    break;
                case "speed+":
                    status.speed += keyValuePairs[key];
                    break;
                case "speed%":
                    status.speed *= keyValuePairs[key];
                    break;
                case "digSpeed+":
                    status.digSpeed += keyValuePairs[key];
                    break;
                case "digSpeed%":
                    status.digSpeed *= keyValuePairs[key];
                    break;
                case "crawlUpSpeed+":
                    status.crawlUpSpeed += keyValuePairs[key];
                    break;
                case "crawlUpSpeed%":
                    status.crawlUpSpeed *= keyValuePairs[key];
                    break;
            }

        }
    }
}
