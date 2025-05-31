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

        public bool IsTutorialDay
        {
            get => date == 0;
        }

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

        /// <summary>
        /// 현재 날짜입니다.
        /// 튜토리얼 날짜인 0일부터 시작합니다.
        /// </summary>
        public int date = 0;

        public int hour => (tick / HourTick + HourOffset) % 24;

        public int minute => tick % HourTick / MinuteTick;

        public bool canSleep = false;


        private bool _isPause = false;

        public bool IsPause
        {
            get => _isPause;
            private set
            {
                if (value)
                {
                    Physics2D.simulationMode = SimulationMode2D.Script;
                    _isPause = true;
                    EventManager.Notify(new PausedEvent());
                }
                else
                {
                    Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
                    _isPause = false;
                    EventManager.Notify(new ResumedEvent());
                }
            }
        }

        private float _nextUpdateTime = 0f;

        private int _wantPauseCount = 0;

        public event Action<int> tickUpdated = delegate { };

        public event Action paused;

        public event Action resumed;

        public override void Update()
        {
            /*if (DataBase.data.GetCutSceneController() == null) return; 
            if (DataBase.data.GetCutSceneController().CutSceneIsPlayed() == true) return;*/
            if (IsPause == true) return;
            if (tick >= MaxTick) return;

            if (Input.GetKeyDown(KeyCode.Alpha0))
                tick += 1600;
            if (Input.GetKeyDown(KeyCode.Alpha9))
                tick += 30;
            if (_nextUpdateTime <= 0)
            {
                // 튜토리얼 진행 중에는 시간이 흐르지 않습니다.
                if (!IsTutorialDay)
                {
                    tick += 1;
                    Debug.Log(tick);
                }

                //To do : tick have to change into 1, But should be do before Release. Not now.
                _nextUpdateTime += 1f / tickSpeed;
                tickUpdated?.Invoke(tick);

                if (tick >= MaxTick)
                {
                    Game.Instance.player!.Die();
                    return;
                }
            }

            _nextUpdateTime -= UnityEngine.Time.deltaTime;
        }

        public void SkipToNextDay()
        {
            AddDay(1);
            tick = Morning;
            tickUpdated.Invoke(tick);
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
            if (_wantPauseCount == 0)
            {
                IsPause = true;
            }

            _wantPauseCount++;
        }

        public void ResumeTime()
        {
            if (_wantPauseCount > 0)
            {
                _wantPauseCount--;

                if (_wantPauseCount == 0)
                {
                    IsPause = false;
                }
            }
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
