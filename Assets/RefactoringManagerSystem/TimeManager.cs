using System;
using System.Collections;
using System.Threading.Tasks;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using UnityEngine;
using Object = System.Object;

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
    
        public int hour => (tick / HourTick + HourOffset) % 24;

        public int minute => tick % HourTick / MinuteTick;
    
        public bool canSleep = false;

        public bool isTimeStop = false;

        private float _nextUpdateTime = 0f;

        private PlayerInput _playerInput;
    
        public event Action<int> tickUpdated;

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
                tick += 10;
                //To do : tick have to change into 1, But should be do before Release. Not now.
                _nextUpdateTime += 1f / tickSpeed;
                tickUpdated?.Invoke(tick);

                if (tick >= MaxTick)
                {
                    tick = 0;
                    date++;
                    /*NextDaySpawn.Invoke();*/
                }

            }
        
            _nextUpdateTime -= UnityEngine.Time.deltaTime;
        }

        public void SkipToNextDay()
        {
            AddDay(1);
            tick = Morning;
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
        
        public async void SetTimer(float seconds, Action callback)
        {
            var milliSeconds = (int)(seconds * 1000);
            await Task.Delay(milliSeconds);
            
            callback.Invoke();
        }
    }
}
