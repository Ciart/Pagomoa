using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class QuestUI : SingletonMonoBehaviour<QuestUI>
    {
        public TextMeshProUGUI listText;
        public TextMeshProUGUI[] infoText;
        public GameObject valueBox;
        public GameObject rewardBox;
        public List<AssignQuestData> assignQuestDatas = new List<AssignQuestData>();
        [SerializeField] private Sprite[] _sprites;

        private List<GameObject> _reward = new List<GameObject>();
        private void OnEnable()
        {
            MakeProgressQuestList();
        }
        private void OnDisable()
        {
        }
        private void MakeProgressQuestList()
        {
            int assignCount = assignQuestDatas.Count;
            int progressCount = QuestManager.instance.progressQuests.Count;
            int dif = QuestManager.instance.progressQuests.Count - assignQuestDatas.Count;

            if (assignCount < progressCount)
            {
                for (int i = 0; i < QuestManager.instance.progressQuests.Count - assignQuestDatas.Count; i++)
                {
                    var listText = Instantiate(this.listText, this.listText.transform.parent);
                    assignQuestDatas.Add(listText.GetComponent<AssignQuestData>());
                    listText.GetComponent<AssignQuestData>().assignProgressQuest = QuestManager.instance.progressQuests[progressCount - dif];
                    listText.GetComponent<TextMeshProUGUI>().text = listText.GetComponent<AssignQuestData>().assignProgressQuest.title;
                    listText.gameObject.SetActive(true);
                    dif++;
                }
            }
            else
                return;
        }
        public void MakeQuestValueBox(GameObject quest)
        {
            for (int i = 0; i < quest.GetComponent<AssignQuestData>().assignProgressQuest.elements.Count; i++)
            {
                var elements = quest.GetComponent<AssignQuestData>().assignProgressQuest.elements[i];
                var Box = Instantiate(valueBox, valueBox.transform.parent);
                Box.SetActive(true);
                Box.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    elements.GetQuestSummary() + " (" + elements.GetCompareValueToString() + "/" +
                       elements.GetValueToString() + ")";

                //switch (quest.GetComponent<AssignQuestData>().assignProgressQuest.elements[i].)
                //{
                //    case QuestType.CollectMineral:
                //        var collectMineral = (CollectMineral)quest.GetComponent<AssignQuestData>().assignProgressQuest.elements[i];
                //        var collectMineralBox = Instantiate(valueBox, valueBox.transform.parent);
                //        collectMineralBox.SetActive(true);
                //        collectMineralBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = collectMineral.summary + " (" + collectMineral.compareValue + "/" +
                //            collectMineral.value + ")";
                //        break;
                //    case QuestType.BreakBlock:
                //        var breakBlock = (BreakBlock)quest.GetComponent<AssignQuestData>().assignProgressQuest.elements[i];
                //        var breakBlockBox = Instantiate(valueBox, valueBox.transform.parent);
                //        breakBlockBox.SetActive(true);
                //        breakBlockBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = breakBlock.summary + " (" + breakBlock.compareValue + "/" +
                //            breakBlock.value + ")";
                //        break;
                //}

            }
        }
        public void BasicQuest(GameObject quest)
        {
            infoText[0].text = quest.GetComponent<AssignQuestData>().assignProgressQuest.title;
            infoText[2].text = quest.GetComponent<AssignQuestData>().assignProgressQuest.description;
            //QuestUI.instance.infoText[2].text = quest.GetComponent<AssignQuestData>().assignProgressQuest.quest
        }
        public void CheckGold(GameObject quest)
        {
            switch (quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.gold)
            {
                case 0:
                    break;

                default:
                    MakeRewardBox("Gold", quest);
                    break;
            }
        }
        public void CheckTargetEntity(GameObject quest)
        {
            switch (quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.targetEntity)
            {
                case null:
                    break;

                default:
                    MakeRewardBox("Entity", quest);
                    break;
            }
        }
        private void MakeRewardBox(string target, GameObject quest)
        {
            var Box = Instantiate(rewardBox, rewardBox.transform.parent);
            _reward.Add(Box);

            switch (target)
            {
                case "Gold":
                    GoldText(quest);
                    break;

                case "Entity":
                    EntityText(quest);
                    break;
            }

            rewardBox.gameObject.SetActive(true);
        }
        private void GoldText(GameObject quest)
        {
            _reward[0].GetComponent<QuestRewardUI>().rewardImage.sprite = _sprites[0];
            _reward[0].GetComponent<QuestRewardUI>().rewardText.text = "X " + quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.gold;
        }
        private void EntityText(GameObject quest)
        {
            //_reward[1].GetComponent<QuestRewardUI>().rewardImage.sprite = quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.targetEntity.
            _reward[1].GetComponent<QuestRewardUI>().rewardText.text = "X " + quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.value.ToString();
        }
    }
}
