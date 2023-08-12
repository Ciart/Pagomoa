using System.Collections;
using System.Collections.Generic;
using Constants;
using Worlds;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDigger : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<float, float> DiggingEvent;

        public Direction direction;
        public bool isDig;

        public int drillLevel = 0;
        public int drillTier = 10;

        public int width = 2;
        public int length = 1;

        public TargetBrickRenderer target;

        private int[] drillSpeed = { 10, 20, 40, 80, 200, 5000, 1000000 };
        private int[] drillTierSetting = { 1, 2, 3, 4, 5 };


        private Status _status;

        private bool _canDig = true;
        private float _charging = 0;

        private void Awake()
        {
            _status = GetComponent<Status>();
        }

        public void DrillUprade()
        {
            drillLevel++;
            if (drillLevel < drillSpeed.Length)
            {
                GetComponent<PlayerController>()._initialStatus.digSpeed = drillSpeed[drillLevel];
                GetComponent<Equip>().CalEquipvalue();
            }

            if (drillLevel < drillTierSetting.Length)
                drillTier = drillTierSetting[drillLevel];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                DrillUprade();
                Debug.Log("드릴 업그레이드 완료! 레벨: " + drillLevel + "드릴스피드: " + _status.digSpeed);
            }
        }

        private void FixedUpdate()
        {
            if (isDig && _canDig)
            {
                StartCoroutine(PA(DirectionCheck(), DirectionCheck()));
            }

            target.ChangeTarget(DirectionCheck(width % 2 == 0), width, length, direction is Direction.Left or Direction.Right);
        }

        Vector2Int DirectionCheck(bool a = false)
        {
            Vector3 digVec;
            switch (direction)
            {
                case Direction.Up:
                    digVec = new Vector3(a ? -0.5f : 0f, 1.2f);
                    break;
                case Direction.Left:
                    digVec = new Vector3(-1.2f, a ? -0.5f : 0f);
                    break;
                case Direction.Right:
                    digVec = new Vector3(1.2f, a ? -0.5f : 0f);
                    break;
                case Direction.Down:
                default:
                    digVec = new Vector3(a ? -0.5f : 0f, -1.2f);
                    break;
            }

            return WorldManager.ComputeCoords(transform.position + digVec);
        }

        void ICanDig()
        {
            _canDig = true;
            _charging = 0;
        }

        IEnumerator PA(Vector2Int point1, Vector2Int point2)
        {
            _canDig = false;

            var worldManager = WorldManager.instance;
            var tile1 = worldManager.world.GetBrick(point1.x, point1.y, out _)?.ground;
            var tile2 = worldManager.world.GetBrick(point2.x, point2.y, out _)?.ground;
            if (tile1 || tile2)
            {
                float time1 = 0, time2 = 0;
                if (tile1) time1 = tile1.strength / _status.digSpeed;
                if (tile2) time2 = tile2.strength / _status.digSpeed;

                Vector2Int currentPos1 = point1, currentPos2 = point2;
                while (isDig && (time1 > _charging || time2 > _charging))
                {
                    currentPos1 = DirectionCheck();
                    currentPos2 = DirectionCheck();

                    _charging += Time.fixedDeltaTime;
                    float time = time1 >= time2 ? time1 : time2;
                    DiggingEvent.Invoke(_charging, time);

                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                    // 파던 위치가 달라지면 초기화 및 탈출
                    if ((currentPos1 != point1) || (currentPos2 != point2))
                    {
                        ICanDig();
                        yield break;
                    }
                }

                if (time1 <= _charging)
                {
                    worldManager.BreakGround(point1.x, point1.y, drillTier);
                    _status.hungry -= 5;
                    _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
                }

                if (time2 <= _charging)
                {
                    worldManager.BreakGround(point2.x, point2.y, drillTier);
                    _status.hungry -= 5;
                    _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
                }
            }

            ICanDig();
        }
    }
}