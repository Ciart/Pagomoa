using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Time
{
    public class TimeManager : Manager<TimeManager>
    {
        ~TimeManager()
        {
            EventManager.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
            Task.FromCanceled(CancellationToken.None);
        }

        public const int MinuteTick = 30;

        public const int HourTick = MinuteTick * 60;

        /// <summary>
        /// 새벽 2시까지 입니다.
        /// </summary>
        public const int MaxTick = HourTick * 20;

        public const int HourOffset = 6;

        /// <summary>
        /// 06:00
        /// </summary>
        public const int Morning = 0;

        private int _tick = 0;

        public int tick
        {
            get => _tick;
            set => _tick = value >= MaxTick ? MaxTick : value;
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
            if (DataBase.data.GetCutSceneController().CutSceneIsPlayed() == true) return;
            if (isTimeStop == true) return;
            if (tick >= MaxTick) return;

            if (_nextUpdateTime <= 0)
            {
                tick += 100;
                //To do : tick have to change into 1, But should be do before Release. Not now.
                _nextUpdateTime += 1f / tickSpeed;
                tickUpdated?.Invoke(tick);

                if (tick >= MaxTick)
                {
                    Game.Instance.player.entityController.Die();
                    return;
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
            if (_playerInput) _playerInput.Actable = false;
        }

        public void ResumeTime()
        {
            UnityEngine.Time.timeScale = 1;
            isTimeStop = false;
            if (_playerInput) _playerInput.Actable = true;
        }
        
        public void RegisterTickEvent(Action<int> action)
        {
            if (!tickUpdated.GetInvocationList().Contains(action))
            {
                tickUpdated += action;   
            }
        }
        
        public void UnregisterTickEvent(Action<int> action)
        {
            if (tickUpdated.GetInvocationList().Contains(action))
            {
                Debug.Log(action.Method.Name);
                tickUpdated -= action;
            }
        }
    }
}