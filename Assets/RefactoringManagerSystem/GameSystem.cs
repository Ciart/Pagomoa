using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.RefactoringManagerSystem;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Save;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;

namespace Ciart.Pagomoa.Systems
{
    public enum GameState
    {
        Playing,
        EndDay,
    }

    public class Game : SingletonMonoBehaviour<Game>
    {
        public PlayerController? player;

        public bool hasPowerGemEarth = false;
        public bool isLoadSave = false;

        public DialogueManager Dialogue { get; private set; } = null!;
        public EntityManager Entity { get; private set; } = null!;
        public NewSaveManager Save { get; private set; } = null!;
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
                EventSystem.Notify(new GameStateChangedEvent(_state));
            }
        }

        public void MoveToNextDay()
        {
            Time.SkipToNextDay();
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            player = e.player;
        }

        protected override void Awake()
        {
            base.Awake();

            Dialogue = new DialogueManager();
            Entity = new EntityManager();
            Save = new NewSaveManager();
            Particle = new ParticleManager();
            Quest = new QuestManager();
            Sound = new SoundManager();
            Time = new TimeManager();
            UI = new UIManager();
            World = new WorldManager();
        }

        private void OnEnable()
        {
            EventSystem.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventSystem.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
    }
}
