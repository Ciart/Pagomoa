using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Time;

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

        public TimeManager Time { get; private set; } = null!;

        public EntityManager Entity { get; private set; } = null!;

        
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
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            player = e.player;
        }

        protected override void Awake()
        {
            Time = new TimeManager();
            Entity = new EntityManager();
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
    }
}
