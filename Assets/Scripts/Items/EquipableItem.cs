using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equip item")]
    public class EquipableItem : OldItem
    {
        public bool Equipable;

        public DicList<string, float> AbilityList;

        public override void Active(PlayerStatus stat)
        {
            Equip(stat);
        }
        public virtual void Equip(PlayerStatus playerStatus)
        {
            Dictionary<string, float> keyValuePairs;
            keyValuePairs = ListDictionaryConverter.ToDictionary(AbilityList);

            var player = Game.Instance.player;
            
            foreach (string key in keyValuePairs.Keys)
            {
                switch (key)
                {
                    case "oxygen+":
                        player.MaxOxygen += keyValuePairs[key];
                        break;
                    case "oxygen%":
                        player.MaxOxygen *= keyValuePairs[key];
                        break;
                    case "oxygenRecovery+":
                        playerStatus.oxygenRecovery += keyValuePairs[key];
                        break;
                    case "oxygenRecovery%":
                        playerStatus.oxygenRecovery *= keyValuePairs[key];
                        break;
                    case "oxygenConsume+":
                        playerStatus.oxygenConsume += keyValuePairs[key];
                        break;
                    case "oxygenConsume%":
                        playerStatus.oxygenConsume *= keyValuePairs[key];
                        break;
                    case "hungry+":
                        player.MaxHungry += keyValuePairs[key];
                        break;
                    case "hungry%":
                        player.MaxHungry *= keyValuePairs[key];
                        break;
                    case "hungryRecovery+":
                        playerStatus.hungryRecovery += keyValuePairs[key];
                        break;
                    case "hungryRecovery%":
                        playerStatus.hungryRecovery *= keyValuePairs[key];
                        break;
                    case "hungryConsume+":
                        playerStatus.hungryConsume += keyValuePairs[key];
                        break;
                    case "hungryConsume%":
                        playerStatus.hungryConsume *= keyValuePairs[key];
                        break;
                    case "armor+":
                        playerStatus.armor += keyValuePairs[key];
                        break;
                    case "armor%":
                        playerStatus.armor *= keyValuePairs[key];
                        break;
                    case "attackPower+":
                        playerStatus.attackpower += keyValuePairs[key];
                        break;
                    case "attackPower%":
                        playerStatus.attackpower *= keyValuePairs[key];
                        break;
                    case "speed+":
                        playerStatus.speed += keyValuePairs[key];
                        break;
                    case "speed%":
                        playerStatus.speed *= keyValuePairs[key];
                        break;
                    case "digSpeed+":
                        playerStatus.digSpeed += keyValuePairs[key];
                        break;
                    case "digSpeed%":
                        playerStatus.digSpeed *= keyValuePairs[key];
                        break;
                    case "crawlUpSpeed+":
                        playerStatus.crawlUpSpeed += keyValuePairs[key];
                        break;
                    case "crawlUpSpeed%":
                        playerStatus.crawlUpSpeed *= keyValuePairs[key];
                        break;
                }

            }
        }
    }
}
