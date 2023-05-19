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

    public static class DirectionUtility
    {
        private static readonly Vector2 BaseVector = new Vector2(1f, 1f);
        
        public static Direction ToDirection(Vector2 vector)
        {
            var signedAngle = Vector2.SignedAngle(BaseVector, vector);
            var angle = signedAngle < 0 ? 360 + signedAngle : signedAngle;

            return angle switch
            {
                >= 0 and < 90 => Direction.Up,
                >= 90 and < 180 => Direction.Left,
                >= 180 and < 270 => Direction.Down,
                >= 270 and < 360 => Direction.Right,
                _ => Direction.Down
            };
        }
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

        private static readonly int AnimatorIsDig = Animator.StringToHash("isDig");
        
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
            _animator.SetBool(AnimatorIsDig, isDig);
        }
        
        private void FixedUpdate()
        {
            var mapManager = MapManager.Instance;

            if (isDig && _canDig)
            {
                Vector3Int po1;
                Vector3Int po2;
                if (direction == Direction.Left)
                {
                    var position = transform.position;
                    po1 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x - 1.2f, position.y + 0.3f, position.z));
                    po2 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x - 1.2f, position.y - 0.3f, position.z));
                }
                else if (direction == Direction.Right)
                {
                    var position = transform.position;
                    po1 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x + 1.2f, position.y + 0.3f, position.z));
                    po2 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x + 1.2f, position.y - 0.3f, position.z));
                }
                else
                {
                    po1 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position + new Vector3(0.3f, 0, 0));
                    po2 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position - new Vector3(0.3f, 0, 0));
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

            var mapManager = MapManager.Instance;
            var tile1 = mapManager.GetTile(point1);
            var tile2 = mapManager.GetTile(point2);
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
                    if (direction == Direction.Left)
                    {
                        var position = transform.position;
                        currentPos1 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x - 1.2f, position.y + 0.3f, position.z));
                        currentPos2 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x - 1.2f, position.y - 0.3f, position.z));
                    }
                    else if (direction == Direction.Right)
                    {
                        var position = transform.position;
                        currentPos1 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x + 1.2f, position.y + 0.3f, position.z));
                        currentPos2 = mapManager.groundTilemap.layoutGrid.WorldToCell( new Vector3(position.x + 1.2f, position.y - 0.3f, position.z));
                    }
                    else
                    {
                        currentPos1 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position + new Vector3(0.3f, 0, 0));
                        currentPos2 = mapManager.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position - new Vector3(0.3f, 0, 0));
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
                    mapManager.BreakTile(point1);
                    _status.hungry -= 5;
                    _status.hungryAlter.Invoke(_status.hungry, _status.maxHungry);
                }
                if (time2 <= _charging)
                {
                    mapManager.BreakTile(point2);
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