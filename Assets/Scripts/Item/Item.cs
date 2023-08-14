using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public ItemInfo info;

    public bool Usable = false, Equipable = false, Etc = false;
    public DicList<string, float> AbilityList;
    public virtual void Active(Status status = null)
    {
        try
        {
            Debug.Log(this.name + " Used()");
        }
        catch
        {
            Debug.Log("None");
        }
    }

    public void StatusUpdate(Status status)
    {
        Dictionary<string, float> keyValuePairs;
        keyValuePairs = ListDictionaryConverter.ToDictionary(AbilityList);
        foreach(string key in keyValuePairs.Keys){
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
