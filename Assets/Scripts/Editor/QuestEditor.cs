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
        public string[] typeOptions = new string[] { "정수형", "boolean형", "상수형"};
        
        public string[] intFuncOptions = new string[] { "Entity 수집", "Entity 소모"};
        public string[] boolFuncOptions = new string[] { "Entity 최초획득", "Entity 조우"};
        public string[] floatFuncOptions = new string[] { "Entity ???", "왜 안나와?"};
        

        public override void OnInspectorGUI()
        {
            Quest newQuest = (Quest)target;
            newQuest.questId = EditorGUILayout.IntField("Quest ID", newQuest.questId);
            newQuest.nextQuestId =  EditorGUILayout.IntField("NextQuest ID", newQuest.nextQuestId);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);

            GUILayout.Space(20);
            EditorGUILayout.LabelField("퀘스트 형식 지정");
            typeIndex = EditorGUILayout.Popup("수행할 타입", typeIndex, typeOptions);
            
            switch (typeIndex)
            {
                case 0:
                    functionIndex = EditorGUILayout.Popup("수행할 타입", functionIndex, intFuncOptions);
                    break;
                case 1:
                    functionIndex = EditorGUILayout.Popup("수행할 타입", functionIndex, boolFuncOptions);
                    break;
                case 2:
                    functionIndex = EditorGUILayout.Popup("수행할 타입", functionIndex, floatFuncOptions);
                    break;
            }
            
            if (GUILayout.Button("퀘스트 생성"))
            {
                if (typeIndex == 0 && functionIndex == 0)
                {
                    QuestCollect collect = new QuestCollect("수집 퀘스트 설명", 0);
                    newQuest.questList.Add(collect);
                } else if (typeIndex == 0 && functionIndex == 1)
                {
                    
                }
            }
            if (GUILayout.Button("퀘스트 삭제"))
            {
                newQuest.questList.RemoveAt(newQuest.questList.Count - 1);
            }

            GUILayout.Space(20);
            
            for (int i = 0; i < newQuest.questList.Count; i++)
            {
                if (newQuest.questList[i].GetType() == typeof(QuestCollect))
                {
                    EditorGUILayout.LabelField($"{i+1}번째 수집 퀘스트 정의");
                    newQuest.questList[i].ConvertTo<QuestCollect>().targetObject = 
                        EditorGUILayout.ObjectField("퀘스트 객체", newQuest.questList[i].ConvertTo<QuestCollect>().targetObject, typeof(ScriptableObject), false) as ScriptableObject;
                    newQuest.questList[i].ConvertTo<QuestCollect>().intValue =
                        EditorGUILayout.IntField("수집할 값", newQuest.questList[i].ConvertTo<QuestCollect>().intValue);
                    GUILayout.Space(20);
                }
            }

            // 퀘스트 조건을 먼저 수집인지, 뭐 조우인지, 삭제인지 등등
            // state 만들어서하거나
            // 아니면 팝업, 버튼
            
            // 퀘스트 조건에 따라 퀘스트 타입을 생성하여 퀘스트 리스트에 삽입
            // 필요한 타겟 엔티티 할당으로 목표 표시하게


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