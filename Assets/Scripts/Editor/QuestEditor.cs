using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Quest))]
    [CanEditMultipleObjects]
    public class QuestEditor : UnityEditor.Editor
    {
        public int typeIndex;
        public int functionIndex;
        public string[] questList;

        public override void OnInspectorGUI()
        {
            questList = new string[Enum.GetValues(typeof(QuestType)).Length];
            for (int i = 0; i < Enum.GetValues(typeof(QuestType)).Length; i++)
            {
                questList[i] = Enum.GetNames(typeof(QuestType))[i];
            }
            
            Quest newQuest = (Quest)target;
            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId = EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20);

            typeIndex = EditorGUILayout.Popup("퀘스트 타입 지정",typeIndex ,questList);
            if (GUILayout.Button("퀘스트 추가하기"))
            {
                newQuest.questList.Add(new QuestCondition());
                switch (typeIndex)
                {
                    case 0:
                        var type = Enum.GetName(typeof(QuestType), typeIndex);
                                        
                        break;
                }
            }
            // QuestCondition value는 int, float, bool 저장
            // CondtionType value는 QuestCondition value에 따라 EditiorGui를 알맞게 제공해서 string으로 저장  

            for (int i = 0; i < newQuest.questList.Count; i++)
            {
                newQuest.questList[i].Summary = EditorGUILayout.TextField("퀘스트 설명", newQuest.questList[i].Summary);
                //newQuest.questList[i].value = EditorGUILayout.La
            }
            
            EditorUtility.SetDirty(newQuest);
        }
    }
}