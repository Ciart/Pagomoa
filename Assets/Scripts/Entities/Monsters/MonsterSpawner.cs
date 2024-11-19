using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class MonsterSpawner : MonoBehaviour
    {
        public int land;

        [SerializeField] private GameObject[] monsterType;

        private GameObject _oneByOneMonster;
    
        private int _landMonster;
    
        private GameObject _monsterPrefab;

        private TimeManager _timeManager;
        void Start()
        {
            _landMonster = land - 1;
            _monsterPrefab = monsterType[_landMonster];

            // ToDo 수정 or 삭제
            /*_timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
            _timeManager.NextDaySpawn.AddListener(SpawnMonster);*/
        }

        public void SpawnMonster()
        {
            CheckIsMonster();
        }

        private void CheckIsMonster()
        {
            if (_oneByOneMonster)
            {
                return ;
            }
            else if (!_oneByOneMonster)
            {
                _oneByOneMonster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
            }
        }
    
        /*private void OnDestroy()
    {
        Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
    }*/
    }
}
