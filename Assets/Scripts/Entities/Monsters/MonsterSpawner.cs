using UnityEngine;

namespace Monsters
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private float hp = 100f;
    
        public int land;

        [SerializeField] private GameObject[] _monsterType;

        private GameObject _oneByOneMonster;
    
        private int _landMonster;
    
        private GameObject _monsterPrefab;

        private TimeManager _timeManager;
        void Start()
        {
            _landMonster = land - 1;
            _monsterPrefab = _monsterType[_landMonster];

            _timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
            _timeManager.NextDaySpawn.AddListener(SpawnMonster);
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
