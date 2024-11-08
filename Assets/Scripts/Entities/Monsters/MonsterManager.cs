using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class MonsterManager : MonoBehaviour
    {
        private NightMonsterSpawner _nightSpawner;
    
        private TimeManager _timeManager;

        private bool _isSleepTime;

        void Start()
        {
            _nightSpawner = FindObjectOfType<NightMonsterSpawner>().GetComponent<NightMonsterSpawner>();
            
            //ToDo 수정 or 제거
            /*_timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
        
            _timeManager.MonsterSleep.AddListener(SleepTime);
            _timeManager.MonsterWakeUp.AddListener(AwakeTime);*/
        }
        void FixedUpdate()
        {
            if (_isSleepTime)
            { 
                _nightSpawner.StartNightSpawner();    
            }
        }

        public void SleepTime()
        {
            _isSleepTime = true;
        }

        public void AwakeTime()
        {
            _nightSpawner.KillNightMonsters();
            _isSleepTime = false;
        }
    }
}
