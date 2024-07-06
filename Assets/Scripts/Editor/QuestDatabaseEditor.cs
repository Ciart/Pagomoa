using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems.Dialogue;
using Logger;
using UnityEditor;
using UnityEngine;

namespace Ciart.Pagomoa.Editor
{
    [CustomEditor(typeof(QuestDatabase))]
    public class QuestDatabaseEditor : UnityEditor.Editor
    {
        private QuestDatabase _questDatabase;

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical(new GUIStyle(GUI.skin.window));
            
            EditorGUILayout.LabelField("EntityOrigin Scriptable");
            GUILayout.Space(10);
            
            if (GUILayout.Button("Entity 추가"))
            {
                _questDatabase.entities.Add(null);
                _questDatabase.mapEntityQuests.Add(new QuestDatabase.MapEntityQuest { entityQuests = new List<Quest>() });
            }
            if (GUILayout.Button("Entity 제거"))
            {
                if (_questDatabase.entities.Count == 0) return;
                
                _questDatabase.entities.RemoveAt(_questDatabase.entities.Count - 1);
                _questDatabase.mapEntityQuests.RemoveAt(_questDatabase.mapEntityQuests.Count - 1);
            }
            GUILayout.EndVertical();
            
            GUILayout.Space(10);

            GUILayout.BeginVertical(new GUIStyle(GUI.skin.window));
            for (var i = 0; i < _questDatabase.entities.Count; i++)
            {
                if (_questDatabase.entities.Count == 0) continue;
                
                _questDatabase.entities[i] = (EntityOrigin)EditorGUILayout
                    .ObjectField($"엔티티 {i + 1}", _questDatabase.entities[i], typeof(EntityOrigin), true);

                if (_questDatabase.entities[i] is null) continue;

                _questDatabase.mapEntityQuests[i].entityName = _questDatabase.entities[i].name;

                GUILayout.BeginVertical(new GUIStyle(GUI.skin.window));
                if (GUILayout.Button($"{_questDatabase.entities[i].name} Quest 추가", GUILayout.MaxWidth(200)))
                {
                    _questDatabase.mapEntityQuests[i].entityQuests.Add(null);
                }
                if (GUILayout.Button($"{_questDatabase.entities[i].name} Quest 제거", GUILayout.MaxWidth(200)))
                {
                    if (_questDatabase.mapEntityQuests[i].entityQuests.Count == 0) return;

                    _questDatabase.mapEntityQuests[i].entityQuests.RemoveAt(_questDatabase.mapEntityQuests[i].entityQuests.Count - 1);
                }
                
                GUILayout.Space(10);
                
                for (var j = 0; j < _questDatabase.mapEntityQuests[i].entityQuests.Count; j++)
                {
                    _questDatabase.mapEntityQuests[i].entityQuests[j] = (Quest)EditorGUILayout
                        .ObjectField($"{_questDatabase.mapEntityQuests[i].entityName} Quest {j + 1}", _questDatabase.mapEntityQuests[i].entityQuests[j], typeof(Quest), false);
                }
                
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            GUILayout.EndVertical();

            EditorUtility.SetDirty(_questDatabase);
        }

        public void OnEnable()
        {
            _questDatabase = (QuestDatabase)target;

            _questDatabase.entities ??= new List<EntityOrigin>();
            _questDatabase.mapEntityQuests ??= new List<QuestDatabase.MapEntityQuest>();
        }
    }
}
