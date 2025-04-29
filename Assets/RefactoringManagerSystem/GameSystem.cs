using System;
using System.Diagnostics;
using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.RefactoringManagerSystem;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Save;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;

using UnityEngine.SceneManagement;

namespace Ciart.Pagomoa.Systems
{
    public enum GameState
    {
        Playing,
        EndDay,
    }
    
    public static class Game
    {
        [Obsolete("Game.Instance를 사용하세요.")]
        public static GameSystem instance => GameSystem.Instance;

        public static GameSystem Instance => GameSystem.Instance;
    }

    public class GameSystem : SingletonMonoBehaviour<GameSystem>
    {
        public PlayerController? player;

        public bool hasPowerGemEarth = false;
        public bool isLoadSave = false;

        public EventManager Event { get; private set; } = null!;
        public DialogueManager Dialogue { get; private set; } = null!;
        public EntityManager Entity { get; private set; } = null!;
        public ParticleManager Particle { get; private set; } = null!;
        public QuestManager Quest { get; private set; } = null!;
        public SoundManager Sound { get; private set; } = null!;
        public TimeManager Time { get; private set; } = null!;
        public UIManager UI { get; private set; } = null!;
        public WorldManager World { get; private set; } = null!;

        
        private GameState _state;
        
        public GameState State
        {
            get => _state;
            set
            {
                _state = value;
                EventManager.Notify(new GameStateChangedEvent(_state));
            }
        }

        public void MoveToNextDay()
        {
            Time.SkipToNextDay();
            player!.Health = player.MaxHealth;
            SaveSystem.Instance.Save();
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            player = e.player;
        }

        protected override void Awake()
        {
            base.Awake();
            SceneManager.activeSceneChanged += LoadScene;
            
            Event = new EventManager();
            Dialogue = new DialogueManager();
            Entity = new EntityManager();
            Particle = new ParticleManager();
            Quest = new QuestManager();
            Sound = new SoundManager();
            Time = new TimeManager();
            UI = new UIManager();
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        private void LoadScene(Scene nowScene, Scene loadScene)
        {
            const int game = 1;
            const int title = 0;
            
            if (loadScene.buildIndex == game)
            {
                World = new WorldManager();
                Game.Instance.Entity.Spawn("Player", transform.position);
                if (nowScene.isLoaded == false) return;
                UI.ActiveUI();
                UI.titleUI.gameObject.SetActive(false);
            }
            else if (loadScene.buildIndex == title)
            {
                UI.DeActiveUI();
                if (nowScene.isLoaded == false) return;
                UI.titleUI.gameObject.SetActive(true);
            }
        }
    }
}
