﻿using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    public class WorldRenderer : MonoBehaviour
    {
        public LevelRenderer levelRendererPrefab;

        private List<LevelRenderer> _levelRenderers = new();

        public LevelRenderer GetLevelRenderer(Level level)
        {
            return _levelRenderers.Find(renderer => renderer.level == level);
        }

        private void OnWorldCreated(WorldCreatedEvent e)
        {
            foreach (var levelRenderer in _levelRenderers)
            {
                Destroy(levelRenderer.gameObject);
            }

            _levelRenderers = new List<LevelRenderer>();

            foreach (var level in e.world.levels)
            {
                var levelRenderer = Instantiate(levelRendererPrefab, transform);
                levelRenderer.Init(level);

                if (level != e.world.currentLevel)
                {
                    levelRenderer.gameObject.SetActive(false);
                }

                _levelRenderers.Add(levelRenderer);
            }
        }
        
        private void OnLevelChanged(LevelChangedEvent e)
        {
            foreach (var levelRenderer in _levelRenderers)
            {
                levelRenderer.gameObject.SetActive(levelRenderer.level == e.level);
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener<WorldCreatedEvent>(OnWorldCreated);
            EventManager.AddListener<LevelChangedEvent>(OnLevelChanged);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<WorldCreatedEvent>(OnWorldCreated);
            EventManager.RemoveListener<LevelChangedEvent>(OnLevelChanged);
        }
    }
}
