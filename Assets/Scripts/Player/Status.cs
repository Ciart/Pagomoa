using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player
{
    public class Status : MonoBehaviour
    {
        public float oxygen = 100f;
        public float oxygenConsume = 0.001f;
        public float maxOxygen = 100f;
        public float minOxygen = 0f;
        
        public float hungry = 100f;
        public float maxHungry = 100f;
        public float minHungry = 0f;

        public UnityEvent<float, float> oxygenAlter;
        public UnityEvent<float, float> hungryAlter;

        private MapManager _map;

        private void Awake()
        {
            _map = MapManager.Instance;
        }

        private void UpdateOxygen()
        {
            if (transform.position.y < _map.groundHeight && oxygen >= minOxygen)
            {
                oxygen -= Mathf.Abs(transform.position.y) * oxygenConsume * Time.deltaTime;

                if (oxygen < minOxygen)
                {
                    oxygen = minOxygen;
                }
            }
            else if (oxygen < maxOxygen)
            {
                oxygen += Mathf.Abs(transform.position.y) * oxygenConsume * Time.deltaTime;
            }

            oxygenAlter.Invoke(oxygen, maxOxygen);
        }
        
        private void Update()
        {
            UpdateOxygen();
        }
    }
}
