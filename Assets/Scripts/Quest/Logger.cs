using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quest
{
    public class Logger : MonoBehaviour
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
            Date
        }

        private static Logger _instance;

        public static Logger Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (Logger)FindObjectOfType(typeof(Logger));
                }
                
                return _instance;
            }
        }

        private Dictionary<LoggingGeneral, int> _objectTypeCounts = new Dictionary<LoggingGeneral, int>();

        public void LogObject(LoggingGeneral objectType)
        {
            if (_objectTypeCounts.ContainsKey(objectType))
            {
                _objectTypeCounts[objectType]++;
            }
            else
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
