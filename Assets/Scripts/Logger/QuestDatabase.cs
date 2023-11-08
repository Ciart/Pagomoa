using System.Collections.Generic;
using UnityEngine;

namespace Logger
{
    public class QuestDatabase : MonoBehaviour
    {
        public List<ScriptableObject> quests = new List<ScriptableObject>();

        private static QuestDatabase _instance;

        public static QuestDatabase Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = (QuestDatabase)FindObjectOfType(typeof(QuestDatabase)); 
                }

                return _instance;
            }
        }
    }
}