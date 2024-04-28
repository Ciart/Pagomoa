using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class QuestUI : SingletonMonoBehaviour<QuestUI>
    {
        public TextMeshProUGUI listText;
        public TextMeshProUGUI[] infoText;
        public List<Sprite> npcImages = new List<Sprite>();
        public Image npcImageObject;
        public GameObject valueBox;
        public GameObject rewardBox;
        public List<ProcessQuest> assignQuestDatas = new List<ProcessQuest>();
        [SerializeField] private Sprite[] _sprites;

        private List<GameObject> _reward = new List<GameObject>();
        private void OnEnable()
        {
            //MakeProgressQuestList();
        }
        public void MakeProgressQuestList()
        {
            int assignCount = assignQuestDatas.Count;
            int progressCount = QuestManager.instance.progressQuests.Count;
            int dif = QuestManager.instance.progressQuests.Count - assignQuestDatas.Count;

            if (assignCount < progressCount)
            {
                var text = Instantiate(listText, listText.transform.parent);
                assignQuestDatas.Add(text.GetComponent<AssignQuestData>().assignProgressQuest);
                text.GetComponent<AssignQuestData>().assignProgressQuest = QuestManager.instance.progressQuests[progressCount - dif];
                text.GetComponent<TextMeshProUGUI>().text = text.GetComponent<AssignQuestData>().assignProgressQuest.title;
                text.GetComponent<AssignQuestData>().npcImage = npcImages[npcImages.Count - 1];
                //Debug.Log(text.GetComponent<AssignQuestData>().assignProgressQuest.title);
                text.gameObject.SetActive(true);
                //Debug.Log(QuestManager.instance.progressQuests.Count + "/" +npcImages.Count);
            }
            else
                return;
        }
        public void MakeQuestValueBox(GameObject quest)
        {
            DestroyValueBox();

            for (int i = 0; i < quest.GetComponent<AssignQuestData>().assignProgressQuest.elements.Count; i++)
            {
                var elements = quest.GetComponent<AssignQuestData>().assignProgressQuest.elements[i];
                var Box = Instantiate(valueBox, valueBox.transform.parent);
                Box.SetActive(true);
                Box.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    elements.GetQuestSummary() + " (" + elements.GetCompareValueToString() + "/" +
                       elements.GetValueToString() + ")";
            }
        }
        public void DestroyValueBox()
        {
            for (int i = 0; i < valueBox.transform.parent.childCount - 1; i++)
                Destroy(valueBox.transform.parent.GetChild(i + 1).gameObject);
        }
        public void BasicQuest(GameObject quest)
        {
            infoText[0].text = quest.GetComponent<AssignQuestData>().assignProgressQuest.title;
            infoText[2].text = quest.GetComponent<AssignQuestData>().assignProgressQuest.description;
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
            _reward[1].GetComponent<QuestRewardUI>().rewardImage.sprite = quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.targetEntitySprite;
            _reward[1].GetComponent<QuestRewardUI>().rewardText.text = "X " + quest.GetComponent<AssignQuestData>().assignProgressQuest.reward.value.ToString();
        }
    }
}
