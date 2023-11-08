using Logger;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : UnityEditor.Editor
    {
        public int index;
        public string[] options = new string[] { "수집, 삭제", "조우, 최초 획득", "float형" };

        public override void OnInspectorGUI()
        {
            Quest newQuest = (Quest)target;
            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId =  EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20);
            newQuest.questList.Capacity = EditorGUILayout.IntField("퀘스트 갯수", newQuest.questList.Capacity);
            GUILayout.Space(20);

            for (int i = 0; i < newQuest.questList.Capacity; i++)
            {
                newQuest.script = (MonoScript)EditorGUILayout.ObjectField("퀘스트 타입", newQuest.script, typeof(MonoScript), true);
            
                index = EditorGUILayout.Popup(index, options);

                switch (index)
                {
                    case 0 :
                    
                        break;
                    case 1 :
                        break;
                }
                GUILayout.Space(20);
            }
            




            // Popup EditorGUILayOut
            // https://docs.unity3d.com/ScriptReference/EditorGUILayout.Popup.html

            /*for (int i = 0; i < newQuest.questIntList.Count; i++)
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
            }*/
            
            EditorUtility.SetDirty(newQuest);
        }
    }
}