using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Logger
{
    [Serializable]
    public class QuestManager : MonoBehaviour
    {
        private int[] ActivatedQuests { get; set; }
        
        private static QuestManager _instance;
        public static QuestManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance =  (QuestManager)FindObjectOfType(typeof(QuestManager));
                }
                return _instance;
            }
        }
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("====================================================");
                Debug.Log("1번 int 퀘스트 : " + QuestDatabase.Instance.quests[0].questList[0].questType);
                Debug.Log("1번 questCondition Target : " + QuestDatabase.Instance.quests[0].questList[0].questCondition.Target);
                Debug.Log("1번 questCondition TypeValue : " + QuestDatabase.Instance.quests[0].questList[0].questCondition.TypeValue);
                Debug.Log("1번 목표 설정 값 : " + QuestDatabase.Instance.quests[0].questList[0].value);
                Debug.Log("1번 목표 : " + QuestDatabase.Instance.quests[0].questList[0].targetEntity.name);
                Debug.Log("====================================================");
                Debug.Log("2번 float 퀘스트 : " + QuestDatabase.Instance.quests[0].questList[1].questType);
                Debug.Log("2번 questCondition Target : " + QuestDatabase.Instance.quests[0].questList[1].questCondition.Target);
                Debug.Log("2번 questCondition TypeValue : " + QuestDatabase.Instance.quests[0].questList[1].questCondition.TypeValue);
                Debug.Log("2번 목표 설정 값 : " + QuestDatabase.Instance.quests[0].questList[1].value);
                Debug.Log("2번 목표 : " + QuestDatabase.Instance.quests[0].questList[1].targetEntity.name);
                Debug.Log("====================================================");
                Debug.Log("3번 bool 퀘스트 : " + QuestDatabase.Instance.quests[0].questList[2].questType);
                Debug.Log("3번 questCondition Target : " + QuestDatabase.Instance.quests[0].questList[2].questCondition.Target);
                Debug.Log("3번 questCondition TypeValue : " + QuestDatabase.Instance.quests[0].questList[2].questCondition.TypeValue);
                Debug.Log("3번 목표 설정 값 : " + QuestDatabase.Instance.quests[0].questList[2].value);
                Debug.Log("3번 목표 : " + QuestDatabase.Instance.quests[0].questList[2].targetEntity.name);
                Debug.Log("====================================================");
            }
        }
    }   
}
