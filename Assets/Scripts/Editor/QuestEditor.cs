using System;
using System.Linq;
using Logger;
using PlasticGui;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Quest<>))]
    public class QuestEditor<T> : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Quest<T> newQuest = (Quest<T>)target;

            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId = EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20f);
            
            // Popup EditorGUILayOut
            // https://docs.unity3d.com/ScriptReference/EditorGUILayout.Popup.html

            if (GUILayout.Button("문자열 목록 추가"))
            {
                newQuest.questStringDic.Add("new Key", "new Value");
            }
            if (GUILayout.Button("불 목록 추가"))
            {
                newQuest.questBoolDic.Add("new Key", false);
            }
            if (GUILayout.Button("정수 목록 추가"))
            {
                newQuest.questIntDic.Add("new Key", 0);
            }
            if (GUILayout.Button("상수 목록 추가"))
            {
                newQuest.questFloatDic.Add("new Key", 0.0f);
            }
            
            

            /*if (GUILayout.Button("하단부터 퀘스트 목록 제거") && newQuest.stringList.Count > 0)
            {
                newQuest.stringList.RemoveAt(newQuest.stringList.Count - 1);
            }*/

            EditorUtility.SetDirty(newQuest);
        }
    }
}