using System;
using System.Text;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using Logger.ForEditorBaseScripts;
using UnityEditor;
using UnityEngine;

namespace Ciart.Pagomoa.Editor
{
    [CustomEditor(typeof(Quest))]
    [CanEditMultipleObjects]
    public class QuestEditor : UnityEditor.Editor
    {
        private int _listIndex;
        private int _typeIndex;
        private readonly string[] _questList = new string[Enum.GetValues(typeof(QuestType)).Length];
        private readonly ConditionDictionary _conditionDic = new();
        private readonly string[] _boolList = { "false", "true" };

        public override void OnInspectorGUI()
        {
            
            Quest newQuest = (Quest)target;
            
            GUILayout.BeginVertical("퀘스트 id", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuest.id = EditorGUILayout.TextField("Quest ID", newQuest.id);
            GUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            GUILayout.BeginVertical("선행 퀘스트 id", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            for (int i = 0; i < newQuest.prevQuestIds.Count; i++)
            {
                newQuest.prevQuestIds[i] = EditorGUILayout.TextField(i+1 + ". PrevQuest ID", newQuest.prevQuestIds[i]);    
            }
            
            if( newQuest.prevQuestIds.Count == 0) 
                EditorGUILayout.LabelField("There is no Prev Quest.");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("+"))
            {
                newQuest.prevQuestIds.Add("");
            }

            if (GUILayout.Button("-"))
            {
                if (newQuest.prevQuestIds.Count == 0) return;
                newQuest.prevQuestIds.RemoveAt(newQuest.prevQuestIds.Count - 1);
            }
            GUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            GUILayout.BeginVertical("퀘스트 설명", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuest.title = EditorGUILayout.TextField("퀘스트 제목", newQuest.title);
            newQuest.description = EditorGUILayout.TextField("퀘스트 설명", newQuest.description);
            GUILayout.EndVertical();

            GUILayout.Space(20);
            
            GUILayout.BeginVertical("퀘스트 보상", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuest.reward.gold = EditorGUILayout.IntField("보상 골드", newQuest.reward.gold);
            newQuest.reward.targetEntity = (Item)EditorGUILayout.ObjectField("보상 엔티티", newQuest.reward.targetEntity, typeof(Item), true);
            newQuest.reward.targetEntitySprite = (Sprite)EditorGUILayout.ObjectField("보상 엔티티 sprite", newQuest.reward.targetEntitySprite, typeof(Sprite), true);
            newQuest.reward.value = EditorGUILayout.IntField("엔티티 보상 개수", newQuest.reward.value);
            GUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            GUILayout.BeginVertical("퀘스트 대화", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuest.startPrologue = (TextAsset)EditorGUILayout.ObjectField("Quest Start Dialogue",
                newQuest.startPrologue, typeof(TextAsset), true);
            newQuest.completePrologue = (TextAsset)EditorGUILayout.ObjectField("Quest Complete Dialogue",
                newQuest.completePrologue, typeof(TextAsset), true);
            GUILayout.EndVertical();
            
            GUILayout.Space(20);
            
            for (int i = 0; i < Enum.GetValues(typeof(QuestType)).Length; i++)
            {
                _questList[i] = Enum.GetNames(typeof(QuestType))[i];
            }
            
            _typeIndex = EditorGUILayout.Popup("퀘스트 타입 지정",_typeIndex ,_questList);
            
            if (GUILayout.Button("퀘스트 추가하기"))
            {
                if (newQuest.questList.Count != 0 && newQuest.questList.Count > 0) _listIndex++;

                var instanceQuestCondition = new QuestCondition
                {
                    value = "",
                    conditionType = new ConditionType()
                };

                newQuest.questList.Add(instanceQuestCondition);

                var conTarget = new ConditionType(); 
                var conTypeValue = new ConditionType(); 
                
                foreach (var conditionType in _conditionDic.typeDictionary)
                {
                    if (conditionType.Key.ToString() == Enum.GetName(typeof(QuestType), _typeIndex))
                    {
                        conTarget.target = conditionType.Value.target;
                        conTypeValue.typeValue = conditionType.Value.typeValue;
                    }
                }

                newQuest.questList[_listIndex].conditionType.target = conTarget.target;
                newQuest.questList[_listIndex].conditionType.typeValue = conTypeValue.typeValue; 
                QuestType[] questTypes = (QuestType[])Enum.GetValues(typeof(QuestType));
                newQuest.questList[_listIndex].questType = questTypes[_typeIndex];
            }
            if (GUILayout.Button("퀘스트 제거하기"))
            {
                if (newQuest.questList.Count == 0) return;
                
                if (newQuest.questList.Count != 1 && newQuest.questList.Count > 0) _listIndex--;
                
                newQuest.questList.RemoveAt(newQuest.questList.Count - 1);
            }
            
            GUILayout.Space(20);
            
            if (newQuest.questList == null || newQuest.questList.Count == 0) return;
            
            for (int i = 0; i < newQuest.questList.Count; i++)
            {
                GUILayout.BeginVertical($"{i+1}번째 퀘스트 목록", new GUIStyle(GUI.skin.window));
                
                EditorGUILayout.LabelField("퀘스트 타입", newQuest.questList[i].questType.ToString());

                if (newQuest.questList[i].questType == QuestType.CollectMineral)
                {
                    newQuest.questList[i].targetEntity = (MineralItem)EditorGUILayout.ObjectField($"타겟 엔티티 {newQuest.questList[i].conditionType.target.ToString()}"
                        ,newQuest.questList[i].targetEntity , typeof(MineralItem), true);
                } else if (newQuest.questList[i].questType == QuestType.BreakBlock)
                {
                    newQuest.questList[i].targetEntity = (Ground)EditorGUILayout.ObjectField($"타겟 엔티티 {newQuest.questList[i].conditionType.target.ToString()}"
                        ,newQuest.questList[i].targetEntity , typeof(Ground), true);
                }
                
                /*newQuest.questList[i].targetEntity = (ScriptableObject)EditorGUILayout.ObjectField($"타겟 엔티티 {newQuest.questList[i].conditionType.target.ToString()}"
                    ,newQuest.questList[i].targetEntity , typeof(ScriptableObject), true);*/
                
                EditorGUILayout.LabelField("타입 값", newQuest.questList[i].conditionType.typeValue);

                newQuest.questList[i].summary = EditorGUILayout.TextField("퀘스트 설명", newQuest.questList[i].summary);
                
                if (newQuest.questList[i].conditionType.typeValue == "int")
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
                } else if (newQuest.questList[i].conditionType.typeValue == "float")
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
                } else if (newQuest.questList[i].conditionType.typeValue == "bool")
                {
                    var temp = 0;
                    temp = EditorGUILayout.Popup("목표 달성 조건 설정", newQuest.questList[i].value == "true" ? 1 : 0, _boolList);
                    newQuest.questList[i].value = temp == 1 ? _boolList[1] : _boolList[0];
                }
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            
            EditorUtility.SetDirty(newQuest);
        }
    }
}