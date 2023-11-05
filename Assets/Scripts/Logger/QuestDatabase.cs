using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    public class QuestDatabase<T> : MonoBehaviour
    {
        public List<Quest<Dictionary<string, T>>> playerQuests = new List<Quest<Dictionary<string, T>>>();

        private static QuestDatabase<T> _instance;

        public static QuestDatabase<T> Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (QuestDatabase<T>)FindObjectOfType(typeof(QuestDatabase<T>));
                }

                return _instance;
            }
        }
    }
}