using System;
using Ciart.Pagomoa.Entities.Monsters;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Systems.Time
{
    public class TimeManager : SingletonMonoBehaviour<TimeManager>
    {
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

        private float nextUpdateTime = 0f;

        private string season = "";
        private bool eventTakePlace = true;

        private PlayerInput playerInput;
    
        public event Action<int> tickUpdated;

        [HideInInspector] public UnityEvent NextDaySpawn;
        [HideInInspector] public UnityEvent MonsterSleep;
        [HideInInspector] public UnityEvent MonsterWakeUp;
        [HideInInspector] public UnityEvent<FadeState> FadeEvent;

        protected override void Awake()
        {
            base.Awake();
            MonsterSleep.AddListener(DayMonster.GetSleep);
            MonsterWakeUp.AddListener(DayMonster.AwakeSleep);
            MonsterWakeUp.AddListener(NightMonster.TimeToBye);
            
        }

        private void OnEnable()
        {
            EventManager.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;
            playerInput = player.GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (nextUpdateTime <= 0)
            {
                tick += 1;
                nextUpdateTime += 1f / tickSpeed;
                tickUpdated?.Invoke(tick);

                if (tick >= MaxTick)
                {
                    tick = 0;
                    date++;
                    NextDaySpawn.Invoke();
                }

                if (season != GetSeasonForPlayer())
                {
                    season = GetSeasonForPlayer();
                    EventTime();
                }
            }
        
            nextUpdateTime -= UnityEngine.Time.deltaTime;
        }

        private void EventTime()
        {
            if (eventTakePlace == true) return;

            if (season == "MiddleNight")
            {
                canSleep = true;
                eventTakePlace = true;
                MonsterSleep.Invoke();
            }

            if (season == "Morning")
            {
                canSleep = false;
                eventTakePlace = true;
                MonsterWakeUp.Invoke();
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

            NextDaySpawn.Invoke();
            MonsterWakeUp.Invoke();
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

        public void AddTime(int hour, int minute)
        {
            tick += (hour * HourTick) + (minute * MinuteTick);
        }
    
        public void SetTime(int hour, int minute)
        {
            tick = ((hour - HourOffset) * HourTick) + (minute * MinuteTick);
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
            if(playerInput) playerInput.Actable = false;
        }

        public void ResumeTime()
        {
            UnityEngine.Time.timeScale = 1;
            isTimeStop = false;
            if (playerInput) playerInput.Actable = true;
        }
    }
}