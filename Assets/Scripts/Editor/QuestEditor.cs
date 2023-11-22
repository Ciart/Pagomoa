using System;
using System.Collections.Generic;
using System.Text;
using Logger;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Quest))]
    [CanEditMultipleObjects]
    public class QuestEditor : UnityEditor.Editor
    {
        public int listIndex;
        public int typeIndex;
        public string[] questList = new string[Enum.GetValues(typeof(QuestType)).Length];
        public ConditionDictionary conditionDic = new ConditionDictionary();
        private readonly string[] _boolList = new[] { "true", "false" };
        

        private void SetQuestType(Quest newQuest)
        {
            var type = Enum.GetName(typeof(QuestType), typeIndex);

            foreach (var conditionType in conditionDic.typeDictionary)
            {
                if (conditionType.Key.ToString() == Enum.GetName(typeof(QuestType), typeIndex))
                {
                    newQuest.questList[listIndex].questCondition.Target = conditionType.Value.Target;
                    newQuest.questList[listIndex].questCondition.TypeValue = conditionType.Value.TypeValue;
                }
            }
            newQuest.questList[listIndex].questType = type;
        }
        
        public override void OnInspectorGUI()
        {
            Quest newQuest = (Quest)target;
            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId = EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20);
            
            for (int i = 0; i < Enum.GetValues(typeof(QuestType)).Length; i++)
            {
                questList[i] = Enum.GetNames(typeof(QuestType))[i];
            }
            typeIndex = EditorGUILayout.Popup("퀘스트 타입 지정",typeIndex ,questList);
            if (GUILayout.Button("퀘스트 추가하기"))
            {
                if (newQuest.questList.Count != 0) listIndex++;    
                
                newQuest.questList.Add(new QuestCondition());
                
                SetQuestType(newQuest);
            }
            if (GUILayout.Button("퀘스트 제거하기"))
            {
                if (newQuest.questList.Count == 0) return;
                
                if (newQuest.questList.Count != 1) listIndex--;
                
                newQuest.questList.RemoveAt(newQuest.questList.Count - 1);
            }
            
            GUILayout.Space(20);
            
            // QuestCondition value는 int, float, bool 저장
            // ConditionType value는 QuestCondition value에 따라 EditorGui를 알맞게 제공해서 string으로 저장
            
            for (int i = 0; i < newQuest.questList.Count; i++)
            {
                EditorGUILayout.LabelField("퀘스트 타입", newQuest.questList[i].questType);
                EditorGUILayout.LabelField("타겟 엔티티", newQuest.questList[i].questCondition.Target.ToString());
                EditorGUILayout.LabelField("타입 값", newQuest.questList[i].questCondition.TypeValue);
                
                newQuest.questList[i].Summary = EditorGUILayout.TextField("퀘스트 설명", newQuest.questList[i].Summary);
                
                if (newQuest.questList[i].questCondition.TypeValue == "int")
                {
                    newQuest.questList[i].value = EditorGUILayout.TextField("수행할 목표 값", newQuest.questList[i].value);
                    StringBuilder result = new StringBuilder();
                    foreach (char c in newQuest.questList[i].value)
                    {
                        if (char.IsDigit(c))
                        {
                            result.Append(c);
                        }
                    }
                    string extractedNumbers = result.ToString();
                    newQuest.questList[i].value = extractedNumbers;
                } else if (newQuest.questList[i].questCondition.TypeValue == "float")
                {
                    newQuest.questList[i].value = EditorGUILayout.TextField("수행할 목표 값", newQuest.questList[i].value);
                    StringBuilder result = new StringBuilder();
                    bool foundDot = false;
                    foreach (char c in newQuest.questList[i].value)
                    {
                        if (char.IsDigit(c) || (c == '.' && !foundDot))
                        {
                            if (c == '.')
                            {
                                foundDot = true;
                            }
                            result.Append(c);
                        }
                    }
                    string extractedNumbers = result.ToString();
                    newQuest.questList[i].value = extractedNumbers;
                } else if (newQuest.questList[i].questCondition.TypeValue == "bool")
                {
                    newQuest.questList[i].boolIndex = EditorGUILayout.Popup("목표 달성 조건 설정", newQuest.questList[i].boolIndex, _boolList);
                    newQuest.questList[i].value = newQuest.questList[i].boolIndex.ToString();
                }
                GUILayout.Space(20);
            }


            EditorUtility.SetDirty(newQuest);
        }
    }
}