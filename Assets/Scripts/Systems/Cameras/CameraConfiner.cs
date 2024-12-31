using Ciart.Pagomoa.Events;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Ciart.Pagomoa
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class CameraConfiner : MonoBehaviour
    {
        private PolygonCollider2D cameraBorderCollider;

        private void Awake()
        {
            if (!TryGetComponent<PolygonCollider2D>(out cameraBorderCollider)) 
            {
                cameraBorderCollider = gameObject.AddComponent<PolygonCollider2D>();
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

        private void OnLevelChanged(LevelChangedEvent e)
        {
            var currentLevel = e.level;
            SetCameraBorder(currentLevel.top, currentLevel.bottom, currentLevel.left, currentLevel.right);
        }

        private void OnWorldCreated(WorldCreatedEvent e)
        {
            var currentLevel = e.world.currentLevel;
            SetCameraBorder(currentLevel.top, currentLevel.bottom, currentLevel.left, currentLevel.right);
        }


        private void SetCameraBorder(int top, int bottom, int left, int right)
        {
            Vector2[] paths = new Vector2[4];
            paths[0] = new Vector2(-left, top);
            paths[1] = new Vector2(right, top);
            paths[2] = new Vector2(right, -bottom);
            paths[3] = new Vector2(-left, -bottom);

            cameraBorderCollider.isTrigger = true;
            cameraBorderCollider.points = paths;

            //GameObject.Find("VirtualCamera").AddComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraBorderCollider;

        }
    }
}
