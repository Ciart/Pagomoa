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
        // 퀘스트 id로 퀘스트 목록 정렬
        // 퀘스트 정렬후 플레이어 퀘스트에 등록
        // 등록된 퀘스트의 진행상황 표시
        // 퀘스트 완료와 종료

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
            if (QuestDatabase.Instance.quests[0].questId == 1)
            {
                QuestCondition<float> a = QuestDatabase.Instance.quests[0].questList[0];
                Debug.Log(a.targetObject);
                Debug.Log(a.Value);
            }
        }
    }   
}
