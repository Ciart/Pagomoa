using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class GameLogger : MonoBehaviour
    {
        [Serializable]
        public enum LoggingQuest
        {
            Brick,
            Mineral,
            Date
        }
        
        [Serializable]
        public enum LoggingGeneral
        {
            Brick, 
            
            Mineral,
            
            Date,
            
            Time,
            
            RemoteControl,
            
            Copper,
            
            Guri,
        }

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
        
        private Dictionary<LoggingGeneral, int> _objectTypeCounts = new Dictionary<LoggingGeneral, int>();
        
        public List<LoggingGeneral> generalKeyList;

        void Start()
        {
            generalKeyList = Enum.GetValues(typeof(LoggingGeneral)).Cast<LoggingGeneral>().ToList();
        }
        
        public void LogObject(LoggingGeneral objectType, int count = 0)
        {
            if (count != 0)
            {
                _objectTypeCounts.Add(objectType, count);
            } else if (_objectTypeCounts.ContainsKey(objectType))
            {
                _objectTypeCounts[objectType]++;
            } else
            {
                _objectTypeCounts.Add(objectType, 1);
            }
        }

        public int GetObjectCount(LoggingGeneral objectType)
        {
            if (_objectTypeCounts.ContainsKey(objectType))
            {
                return _objectTypeCounts[objectType];
            }
            return 0;
        }

    }   
}
