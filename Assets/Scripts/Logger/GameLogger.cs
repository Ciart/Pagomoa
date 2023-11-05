using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logger
{
    public class PlayerQuest<T> : MonoBehaviour
    {
        private bool _clear = false;
        private int _questId;
        private int _nextQuestId;
        private string _description;
        
        private List<Dictionary<string, T>> _quests = new List<Dictionary<string, T>>();

        PlayerQuest(int questId)
        {
            Quest<T> initQuest = null;
            foreach (var quest in QuestDatabase<T>.Instance.playerQuests)
            {
                if (quest.questId == questId)
                {
                    //initQuest = quest;
                }
                else
                {
                    return;
                }
            }
            if (initQuest == null) return;
            
            _clear = initQuest.clear;
            _questId = initQuest.questId;
            _nextQuestId = initQuest.nextQuestId;
            _description = initQuest.description;
            
        }
    }
    
    [Serializable]
    public class GameLogger<T> : MonoBehaviour
    {
        // 퀘스트 id로 퀘스트 목록 정렬
        // 퀘스트 정렬후 플레이어 퀘스트에 등록
        // 등록된 퀘스트의 진행상황 표시
        // 퀘스트 완료와 종료

        private static GameLogger<T> _instance;
        public static GameLogger<T> Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (GameLogger<T>)FindObjectOfType(typeof(GameLogger<T>));
                }
                return _instance;
            }
        }

        private List<PlayerQuest<T>> playerQuests = new List<PlayerQuest<T>>(); 
        
        private Dictionary<string, string> _stringQuest = new Dictionary<string, string>();
        private Dictionary<string, int> _intQuest = new Dictionary<string, int>();
        private Dictionary<string, float> _floatQuest = new Dictionary<string, float>();
        private Dictionary<string, bool> _boolQuest = new Dictionary<string, bool>();

        void Start()
        {
            
        }
        
        
        

    }   
}
