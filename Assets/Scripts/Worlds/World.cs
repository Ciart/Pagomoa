using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Worlds
{
    public class World
    {
        public const int GroundHeight = 0;

        public readonly List<Level> levels = new();

        private int _currentLevelIndex;

        public Level currentLevel => levels[_currentLevelIndex];

        public World()
        {
        }

        public World(WorldSaveData saveData)
        {
            levels = saveData.levels.Select(data => new Level(data)).ToList();
        }

        public WorldSaveData CreateSaveData()
        {
            return new WorldSaveData()
            {
                levels = levels.Select(level => level.CreateSaveData()).ToArray()
            };
        }

        // TODO: WorldManager로 옮겨야 함
        public bool ChangeLevel(string id)
        {
            var index = levels.FindIndex(level => level.id == id);

            if (index == -1)
            {
                return false;
            }

            _currentLevelIndex = index;

            EventSystem.Notify(new LevelChangedEvent(currentLevel));

            return true;
        }
    }
}