using System.Collections;
using System.Collections.Generic;
using Constants;
using Worlds;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDigger : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent<float, float> DiggingEvent;
        public UnityEvent digEndEvent;

        public Direction direction;
        public  bool isDig;
        public float tiredDigSpeedScoop = 5f;

        public int drillLevel = 0;
        public int drillTier = 10;
        private int[] drillSpeed = {10,20,40,80,200,5000,1000000 };
        private int[] drillTierSetting = {1,2,3,4,5 };


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
            }
            if(drillLevel < drillTierSetting.Length)
                drillTier = drillTierSetting[drillLevel];
        }
        private void FixedUpdate()
        {
            if (isDig && _canDig)
                StartCoroutine(PA(DirectionCheck("1"), DirectionCheck("2")));
        }
        Vector2Int DirectionCheck(string oneortwo)
        {
            Vector3 digVec;
            switch (direction)
            {
                case Direction.Left:
                    digVec = (oneortwo == "1") ? new Vector3(-1.2f, 0.3f) : new Vector3(-1.2f, -0.3f);
                    break;
                case Direction.Right:
                    digVec = (oneortwo == "1") ? new Vector3(1.2f, 0.3f) : new Vector3(1.2f, -0.3f);
                    break;
                default:
                    digVec = (oneortwo == "1") ? new Vector3(0.3f, -1.2f) : new Vector3(-0.3f, -1.2f);
                    break;
            }
            return WorldManager.ComputeCoords(transform.position + digVec);
        }
        void ICanDig()
        {
            digEndEvent.Invoke();
            _canDig = true;
            _charging = 0;
        }
        IEnumerator PA(Vector2Int point1, Vector2Int point2)
        {
            _canDig = false;

            var worldManager = WorldManager.instance;
            var tile1 = worldManager.world.GetBrick(point1.x, point1.y, out _)?.ground;
            var tile2 = worldManager.world.GetBrick(point2.x, point2.y, out _)?.ground;

            bool breakable1 = false, breakable2 = false;

            breakable1 = tile1 ? worldManager.CheckBreakable(point1.x, point1.y, drillTier, "drill") : false;
            breakable2 = tile2 ? worldManager.CheckBreakable(point1.x, point1.y, drillTier, "drill") : false;

            if (breakable1 || breakable2)
            {
                float time1 = 0, time2 = 0, totalHungry = 0;

                if (breakable1)
                {
                    time1 = tile1.strength / _status.digSpeed;
                    totalHungry += 0.5f;
                }
                if (breakable2)
                {
                    time2 = tile2.strength / _status.digSpeed;
                    totalHungry += 0.5f;
                }

                if (GetComponent<PlayerController>().Hungry(totalHungry))
                {
                    time1 *= tiredDigSpeedScoop;
                    time2 *= tiredDigSpeedScoop;
                }


                Vector2Int currentPos1 = point1, currentPos2 = point2;
                while (isDig && (time1 > _charging || time2 > _charging))
                {
                    currentPos1 = DirectionCheck("1");
                    currentPos2 = DirectionCheck("2");

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
                if (time1 <= _charging) worldManager.BreakGround(point1.x, point1.y, drillTier, "drill");
                if (time2 <= _charging) worldManager.BreakGround(point2.x, point2.y,  drillTier, "drill");
            }
            ICanDig();
        }
    }
}
