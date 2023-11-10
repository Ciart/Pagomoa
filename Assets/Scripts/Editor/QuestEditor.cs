using Logger;
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
                    QuestCollect collect = new QuestCollect("수집 퀘스트 설명", 0.0f);
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
                newQuest.questList[i].targetObject = (ScriptableObject)EditorGUILayout.ObjectField("기록할 객체",
                    newQuest.questList[i].targetObject, typeof(ScriptableObject), true);
                int inputValue = EditorGUILayout.IntField("기록할 갯수", (int)newQuest.questList[i].Value);
                newQuest.questList[i].Value = inputValue;
            }

            GUILayout.Space(20);

            EditorUtility.SetDirty(newQuest);
        }
    }
}