using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : UnityEditor.Editor
    {
        
        
        public override void OnInspectorGUI()
        {
            Quest newQuest = (Quest)target;

            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId = EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20);
            
            // Popup EditorGUILayOut
            // https://docs.unity3d.com/ScriptReference/EditorGUILayout.Popup.html

            // popup으로 타입 정하고
            // 들어갈 값 정하기

            
            for (int i = 0; i < newQuest.questIntList.Count; i++)
            {
                EditorGUILayout.LabelField($"퀘스트{i+1}");    
                newQuest.questIntList[i].summary = EditorGUILayout.TextField("Summary", newQuest.questIntList[i].summary);
                newQuest.questIntList[i].value = EditorGUILayout.IntField("Int Value", newQuest.questIntList[i].value);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("int형 문자열 목록 추가"))
            {
                QuestType<int> quest = new QuestType<int>("퀘스트 설명", 0);

                newQuest.questIntList.Add(quest);
            }
            if (GUILayout.Button("하단부터 퀘스트 목록 제거") && newQuest.questIntList.Count > 0)
            {
                newQuest.questIntList.RemoveAt(newQuest.questIntList.Count - 1);
            }
            
            
            for (int i = 0; i < newQuest.questFloatList.Count; i++)
            {
                EditorGUILayout.LabelField($"퀘스트{i+1}");    
                newQuest.questFloatList[i].summary = EditorGUILayout.TextField("Summary", newQuest.questFloatList[i].summary);
                newQuest.questFloatList[i].value = EditorGUILayout.FloatField("Float Value", newQuest.questFloatList[i].value);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("float형 문자열 목록 추가"))
            {
                QuestType<float> quest = new QuestType<float>("퀘스트 설명", 0.0f);

                newQuest.questFloatList.Add(quest);
            }
            if (GUILayout.Button("하단부터 퀘스트 목록 제거") && newQuest.questFloatList.Count > 0)
            {
                newQuest.questFloatList.RemoveAt(newQuest.questFloatList.Count - 1);
            }
            
            for (int i = 0; i < newQuest.questBoolList.Count; i++)
            {
                EditorGUILayout.LabelField($"퀘스트{i+1}");    
                newQuest.questBoolList[i].summary = EditorGUILayout.TextField("Summary", newQuest.questBoolList[i].summary);
                newQuest.questBoolList[i].value = EditorGUILayout.Toggle("Bool Value", newQuest.questBoolList[i].value);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("bool형 문자열 목록 추가"))
            {
                QuestType<bool> quest = new QuestType<bool>("퀘스트 설명", false);

                newQuest.questBoolList.Add(quest);
            }
            if (GUILayout.Button("하단부터 퀘스트 목록 제거") && newQuest.questBoolList.Count > 0)
            {
                newQuest.questBoolList.RemoveAt(newQuest.questBoolList.Count - 1);
            }
            
            EditorUtility.SetDirty(newQuest);
        }
    }
}