using System.Collections.Generic;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
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
                _questDatabase.mapEntityQuests.Add(new QuestDatabase.MapEntityQuest { entityQuests = new List<QuestData>() });
            }
            if (GUILayout.Button("Entity 제거"))
            {
                if (_questDatabase.mapEntityQuests.Count == 0) return;
                
                _questDatabase.mapEntityQuests.RemoveAt(_questDatabase.mapEntityQuests.Count - 1);
            }
            GUILayout.EndVertical();
            
            GUILayout.Space(10);

            GUILayout.BeginVertical(new GUIStyle(GUI.skin.window));
            for (var i = 0; i < _questDatabase.mapEntityQuests.Count; i++)
            {
                if (_questDatabase.mapEntityQuests.Count == 0) continue;
                
                _questDatabase.mapEntityQuests[i].entity = (EntityController)EditorGUILayout
                    .ObjectField($"엔티티 {i + 1}", _questDatabase.mapEntityQuests[i].entity, typeof(EntityController), true);

                if (_questDatabase.mapEntityQuests[i].entity is null) continue;
                
                var entity = ResourceSystem.instance.GetEntity(_questDatabase.mapEntityQuests[i].entity.entityId);

                GUILayout.BeginVertical(new GUIStyle(GUI.skin.window));
                if (GUILayout.Button($"{entity.name} Quest 추가", GUILayout.MaxWidth(200)))
                {
                    _questDatabase.mapEntityQuests[i].entityQuests.Add(null);
                }
                if (GUILayout.Button($"{entity.name} Quest 제거", GUILayout.MaxWidth(200)))
                {
                    if (_questDatabase.mapEntityQuests[i].entityQuests.Count == 0) return;

                    _questDatabase.mapEntityQuests[i].entityQuests.RemoveAt(_questDatabase.mapEntityQuests[i].entityQuests.Count - 1);
                }
                
                GUILayout.Space(10);
                
                for (var j = 0; j < _questDatabase.mapEntityQuests[i].entityQuests.Count; j++)
                {
                    _questDatabase.mapEntityQuests[i].entityQuests[j] = (QuestData)EditorGUILayout
                        .ObjectField($"{entity.name} Quest {j + 1}", _questDatabase.mapEntityQuests[i].entityQuests[j], typeof(QuestData), false);
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
            
            _questDatabase.mapEntityQuests ??= new List<QuestDatabase.MapEntityQuest>();
        }
    }
}
