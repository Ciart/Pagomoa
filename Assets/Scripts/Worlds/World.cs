using System.Collections.Generic;
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

        public bool ChangeLevel(string id)
        {
            var index = levels.FindIndex(level => level.id == id);

            if (index == -1)
            {
                return false;
            }

            _currentLevelIndex = index;
            
            EventManager.Notify(new LevelChangedEvent(currentLevel));
            
            return true;
        }
    }
}