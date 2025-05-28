using System.Collections.Generic;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Environments
{
    public class BackgroundController : MonoBehaviour
    {
        public List<GameObject> mainBackgrounds;
        public List<GameObject> yellowDungeonBackgrounds;

        private void SetBackgrounds(string levelId)
        {
            var isMain = levelId == "Main";
            mainBackgrounds.ForEach(bg => bg.SetActive(isMain));
            yellowDungeonBackgrounds.ForEach(bg => bg.SetActive(!isMain));
        }

        private void OnWorldCreated(WorldCreatedEvent e)
        {
            SetBackgrounds(e.world.currentLevel.id);
        }

        private void OnLevelChanged(LevelChangedEvent e)
        {
            SetBackgrounds(e.level.id);
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
