using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
    
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDigger : MonoBehaviour
    {
        public UnityEvent OnDigEvent;

        public UnityEvent<float, float> DiggingEvent;
        public UnityEvent DigSuccessEvent;
        
        public Direction direction;
        public float digSpeed = 10f;

        [SerializeField] public Transform HitPointDown;
        [SerializeField] public Transform HitPointHorizontal;

        private Animator _animator;
        
        private Status _status;

        public  bool isDig;
        private bool _canDig = true;
        private float _charging = 0;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _status = GetComponent<Status>();
            
            if (OnDigEvent == null)
                OnDigEvent = new UnityEvent();
            if (DigSuccessEvent == null)
                DigSuccessEvent = new UnityEvent();
            if (DiggingEvent == null)
                DiggingEvent = new UnityEvent<float, float>();
        }

        private void Update()
        {
            _animator.SetBool("isDig", isDig);
        }
        
        private void FixedUpdate()
        {
            var mapManager = MapManager.Instance;

            if (isDig && _canDig)
            {
                Vector3Int po1;
                Vector3Int po2;
                if (direction == Direction.Down)
                {
                    po1 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position + new Vector3(0.3f, 0, 0));
                    po2 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position - new Vector3(0.3f, 0, 0));
                }
                else
                {
                    po1 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position + new Vector3(0, 0.3f, 0));
                    po2 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position - new Vector3(0, 0.3f, 0));
                }
                StartCoroutine(PA(po1, po2));

            }
        }
        void ICanDig()
        {
            _canDig = true;
            _charging = 0;
            OnDigEvent.Invoke();
        }
        IEnumerator PA(Vector3Int point1, Vector3Int point2)
        {
            _canDig = false;

            var Ground = MapManager.Instance;
            var tile1 = Ground.GetTile(point1);
            var tile2 = Ground.GetTile(point2);
            if (tile1 || tile2)
            {
                Debug.Log("굴착시작!");
                float time1 = 0;
                float time2 = 0;
                if (tile1) time1 = tile1.strength / digSpeed;
                if (tile2) time2 = tile2.strength / digSpeed;

                Vector3Int currentPos1 = point1;
                Vector3Int currentPos2 = point2;
                while (isDig && (time1 > _charging || time2 > _charging))
                {
                    //Debug.Log("굴착중!");
                    if (direction == Direction.Down)
                    {
                        currentPos1 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position + new Vector3(0.3f, 0, 0));
                        currentPos2 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position - new Vector3(0.3f, 0, 0));
                    }
                    else
                    {
                        currentPos1 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position + new Vector3(0, 0.3f, 0));
                        currentPos2 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position - new Vector3(0, 0.3f, 0));
                    }

                    _charging += Time.fixedDeltaTime;
                    float time = time1 >= time2 ? time1 : time2;
                    DiggingEvent.Invoke(_charging, time);

                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                    // 파던 위치가 달라지면 초기화 및 탈출
                    if ((currentPos1 != point1) || (currentPos2 != point2))
                    {
                        Debug.Log("파던 위치가 달라졌잖아! 취소!");
                        ICanDig();
                        yield break;
                    }
                }
                if (time1 <= _charging)
                {
                    Ground.BreakTile(point1);
                    _status.hungry -= 5;
                    _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
                }
                if (time2 <= _charging)
                {
                    Ground.BreakTile(point2);
                    _status.hungry -= 5;
                    _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
                }
                ICanDig();
            }
            else
                ICanDig();
            //Debug.Log("없어!");
        }
    }
}