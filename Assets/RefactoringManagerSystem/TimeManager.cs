using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;

namespace Ciart.Pagomoa.Systems.Time
{
    public class TimeManager : PManager<TimeManager>
    {
        ~TimeManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }
        
        public const int MinuteTick = 30;
    
        public const int HourTick = MinuteTick * 60;
    
        public const int MaxTick = HourTick * 24;

        public const int HourOffset = 6;
    
        /// <summary>
        /// 06:00
        /// </summary>
        public const int Morning = 0;

        private int _tick = 0;
        
        public int tick {
            get => _tick;
            set => _tick = value % MaxTick;
        }
    
        /// <summary>
        /// 1초 당 틱 수
        /// </summary>
        public int tickSpeed = 30;

        public int date = 1;
    
        public int hour => tick / HourTick + HourOffset;

        public int minute => tick % HourTick / MinuteTick;
    
        public bool canSleep = false;

        public bool isTimeStop = false;

        private float _nextUpdateTime = 0f;

        private string _season = "";
        private bool _eventTakePlace = true;

        private PlayerInput _playerInput;
    
        public event Action<int> tickUpdated;

        /*[HideInInspector] public UnityEvent NextDaySpawn;
        [HideInInspector] public UnityEvent MonsterSleep;
        [HideInInspector] public UnityEvent MonsterWakeUp;
        [HideInInspector] public UnityEvent<FadeState> FadeEvent;*/
        
        public override void Awake()
        {
            /*base.Awake();
            MonsterSleep.AddListener(DayMonster.GetSleep);
            MonsterWakeUp.AddListener(DayMonster.AwakeSleep);
            MonsterWakeUp.AddListener(NightMonster.TimeToBye);*/
        }

        public override void Start()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;
            _playerInput = player.GetComponent<PlayerInput>();
        }

        public override void Update()
        {
            if (_nextUpdateTime <= 0)
            {
                tick += 1;
                _nextUpdateTime += 1f / tickSpeed;
                tickUpdated?.Invoke(tick);

                if (tick >= MaxTick)
                {
                    tick = 0;
                    date++;
                    /*NextDaySpawn.Invoke();*/
                }

                if (_season != GetSeasonForPlayer())
                {
                    _season = GetSeasonForPlayer();
                    EventTime();
                }
            }
        
            _nextUpdateTime -= UnityEngine.Time.deltaTime;
        }

        private void EventTime()
        {
            if (_eventTakePlace == true) return;

            if (_season == "MiddleNight")
            {
                canSleep = true;
                _eventTakePlace = true;
                /*MonsterSleep.Invoke();*/
            }

            if (_season == "Morning")
            {
                canSleep = false;
                _eventTakePlace = true;
                /*MonsterWakeUp.Invoke();*/
            }
        }
    
        // private void ReturnToBase()
        // {
        //     Vector3 returnPosition = new Vector3(31.7f, 4, 0);
        //     gameObject.transform.position = returnPosition;
        // }
    
        public void Sleep()
        {
            // FadeEvent.Invoke(FadeState.FadeInOut);
            EventManager.Notify(new FadeEvent(FadeState.FadeInOut));
            AddDay(1);
            tick = Morning;

            /*NextDaySpawn.Invoke();
            MonsterWakeUp.Invoke();*/
        }

        public static string GetSeasonForMonster()
        {
            var tick = instance.tick;
            
            if (tick >= 0 && tick < 6 * HourTick)
                return "Night";
            else if (tick < 22 * HourTick)
                return "Day";
            else
                return "Night";
        }

        public static string GetSeasonForPlayer()
        {
            var tick = instance.tick;
            
            if (6 * HourTick < tick && tick < 12 * HourTick)
                return "Morning";
            else if (tick < 18 * HourTick)
                return "Afternoon";
            else if (tick < 22 * HourTick)
                return "Night";
            else
                return "MiddleNight";
        }

        public void AddTime(int hourToAdd, int minuteToAdd)
        {
            tick += (hourToAdd * HourTick) + (minuteToAdd * MinuteTick);
        }
    
        public void SetTime(int hourToAdd, int minuteToAdd)
        {
            tick = ((hourToAdd - HourOffset) * HourTick) + (minuteToAdd * MinuteTick);
        }
    
        public void SetDay(int day)
        {
            date = day;
        }
    
        public void AddDay(int day)
        {
            date += day;
        }

        public void PauseTime()
        {
            UnityEngine.Time.timeScale = 0;
            isTimeStop = true;
            if(_playerInput) _playerInput.Actable = false;
        }

        public void ResumeTime()
        {
            UnityEngine.Time.timeScale = 1;
            isTimeStop = false;
            if (_playerInput) _playerInput.Actable = true;
        }
    }
}
