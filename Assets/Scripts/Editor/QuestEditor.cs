using System;
using System.Collections.Generic;
using System.Text;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.Worlds;
using Logger.ForEditorBaseScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ciart.Pagomoa.Editor
{
    [CustomEditor(typeof(QuestData))]
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
            QuestData newQuestData = (QuestData)target;
            
            GUILayout.BeginVertical("퀘스트 id", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuestData.id = EditorGUILayout.TextField("Quest ID", newQuestData.id);
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical("선행 퀘스트 등록", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            
            for (int i = 0; i < newQuestData.prevQuestData.Count; i++)
            {
                newQuestData.prevQuestData[i] = (QuestData)EditorGUILayout.ObjectField("선행 퀘스트", newQuestData.prevQuestData[i], typeof(QuestData), false);
                if (!newQuestData.prevQuestData[i]) continue;

                newQuestData.prevQuestIds.Add(newQuestData.prevQuestData[i].id);
            }

            if (newQuestData.prevQuestIds.Count == 0)
                EditorGUILayout.LabelField("There is no Prev Quest.");

            GUILayout.Space(10);

            if (GUILayout.Button("+"))
            {
                newQuestData.prevQuestData.Add(null);
            }

            if (GUILayout.Button("-"))
            {
                if (newQuestData.prevQuestIds.Count == 0) return;
                newQuestData.prevQuestIds.RemoveAt(newQuestData.prevQuestIds.Count - 1);
                newQuestData.prevQuestData.RemoveAt(newQuestData.prevQuestIds.Count - 1);
            }
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical("퀘스트 설명", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuestData.title = EditorGUILayout.TextField("퀘스트 제목", newQuestData.title);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("퀘스트 설명");
            newQuestData.description = EditorGUILayout.TextArea(newQuestData.description, GUILayout.Height(100));
            if (string.IsNullOrEmpty(newQuestData.description)) newQuestData.description = "";
            GUILayout.EndVertical();
            
            GUILayout.Space(20);

            GUILayout.BeginVertical("퀘스트 보상", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuestData.reward.gold = EditorGUILayout.IntField("보상 골드", newQuestData.reward.gold);
            newQuestData.reward.itemID = EditorGUILayout.TextField("보상 아이템", newQuestData.reward.itemID);
            newQuestData.reward.itemSprite = (Sprite)EditorGUILayout.ObjectField("보상 아이템 sprite", newQuestData.reward.itemSprite, typeof(Sprite), true);
            newQuestData.reward.value = EditorGUILayout.IntField("엔티티 보상 개수", newQuestData.reward.value);
            GUILayout.EndVertical();

            GUILayout.Space(20);

            GUILayout.BeginVertical("퀘스트 대화", new GUIStyle(GUI.skin.window));
            GUILayout.Space(10);
            newQuestData.startPrologue = (TextAsset)EditorGUILayout.ObjectField("Quest Start Dialogue",
                newQuestData.startPrologue, typeof(TextAsset), true);
            newQuestData.completePrologue = (TextAsset)EditorGUILayout.ObjectField("Quest Complete Dialogue",
                newQuestData.completePrologue, typeof(TextAsset), true);
            GUILayout.EndVertical();

            GUILayout.Space(20);

            for (int i = 0; i < Enum.GetValues(typeof(QuestType)).Length; i++)
            {
                _questList[i] = Enum.GetNames(typeof(QuestType))[i];
            }

            _typeIndex = EditorGUILayout.Popup("퀘스트 타입 지정", _typeIndex, _questList);

            if (GUILayout.Button("퀘스트 추가하기"))
            {
                if (newQuestData.questList.Count != 0 && newQuestData.questList.Count > 0) _listIndex++;

                var instanceQuestCondition = new QuestConditionData
                {
                    value = "",
                    conditionType = new ConditionType()
                };

                newQuestData.questList.Add(instanceQuestCondition);

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

                newQuestData.questList[_listIndex].conditionType.target = conTarget.target;
                newQuestData.questList[_listIndex].conditionType.typeValue = conTypeValue.typeValue;
                QuestType[] questTypes = (QuestType[])Enum.GetValues(typeof(QuestType));
                newQuestData.questList[_listIndex].questType = questTypes[_typeIndex];
            }
            if (GUILayout.Button("퀘스트 제거하기"))
            {
                if (newQuestData.questList.Count == 0) return;

                if (newQuestData.questList.Count != 1 && newQuestData.questList.Count > 0) _listIndex--;

                newQuestData.questList.RemoveAt(newQuestData.questList.Count - 1);
            }

            GUILayout.Space(20);

            if (newQuestData.questList == null || newQuestData.questList.Count == 0) return;

            for (int i = 0; i < newQuestData.questList.Count; i++)
            {
                GUILayout.BeginVertical($"{i + 1}번째 퀘스트 목록", new GUIStyle(GUI.skin.window));

                EditorGUILayout.LabelField("퀘스트 타입", newQuestData.questList[i].questType.ToString());
                GUILayout.Space(10);
                newQuestData.questList[i].summary = EditorGUILayout.TextField("퀘스트 설명", newQuestData.questList[i].summary);
                EditorGUILayout.LabelField("타입 값", newQuestData.questList[i].conditionType.typeValue);
                
                switch (newQuestData.questList[i].questType)
                {
                    case QuestType.CollectItem:
                    case QuestType.HasItem:
                        newQuestData.questList[i].targetID = EditorGUILayout.TextField($"수집할 아이템 ID"
                            , newQuestData.questList[i].targetID);
                        break;
                    case QuestType.UseItem:
                        newQuestData.questList[i].targetID = EditorGUILayout.TextField($"사용할 아이템 ID"
                            , newQuestData.questList[i].targetID);
                        break;
                    case QuestType.BreakBlock:
                        newQuestData.questList[i].targetID = EditorGUILayout.TextField($"파괴할 블럭 ID"
                            , newQuestData.questList[i].targetID);
                        break;
                }
                
                if (newQuestData.questList[i].conditionType.typeValue == "int")
                {
                    newQuestData.questList[i].value = EditorGUILayout.TextField("수행할 목표 값", newQuestData.questList[i].value);
                    StringBuilder result = new StringBuilder();
                    foreach (char c in newQuestData.questList[i].value)
                    {
                        if (char.IsDigit(c))
                        {
                            result.Append(c);
                        }
                    }
                    string extractedNumbers = result.ToString();
                    newQuestData.questList[i].value = extractedNumbers;
                }
                else if (newQuestData.questList[i].conditionType.typeValue == "bool")
                {
                    var temp = 0;
                    temp = EditorGUILayout.Popup("목표 달성 조건 설정", newQuestData.questList[i].value == "true" ? 1 : 0, _boolList);
                    newQuestData.questList[i].value = temp == 1 ? _boolList[1] : _boolList[0];
                } /*else if (newQuestData.questList[i].conditionType.typeValue == "float")
                {
                    newQuestData.questList[i].value = EditorGUILayout.TextField("수행할 목표 값", newQuestData.questList[i].value);
                    StringBuilder result = new StringBuilder();
                    bool foundDot = false;
                    foreach (char c in newQuestData.questList[i].value)
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
                    newQuestData.questList[i].value = extractedNumbers;
                } */

                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            
            EditorUtility.SetDirty(newQuestData);
        }
    }
}
