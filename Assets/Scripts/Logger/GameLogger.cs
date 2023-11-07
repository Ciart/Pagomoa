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
    }
    
    [Serializable]
    public class GameLogger : MonoBehaviour
    {
        // 퀘스트 id로 퀘스트 목록 정렬
        // 퀘스트 정렬후 플레이어 퀘스트에 등록
        // 등록된 퀘스트의 진행상황 표시
        // 퀘스트 완료와 종료

        private static GameLogger _instance;
        public static GameLogger Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (GameLogger)FindObjectOfType(typeof(GameLogger));
                }
                return _instance;
            }
        }

        void Start()
        {
            
        }
        
        
        

    }   
}
