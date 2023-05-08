using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class Dig : MonoBehaviour
    {
        public UnityEvent OnDigEvent;

        public UnityEvent<float, float> DiggingEvent;
        public UnityEvent DigSuccessEvent;

        private PlayerController player;
        public float direction;
        public float digSpeed = 10f;

        [SerializeField] public Transform HitPointDown;
        [SerializeField] public Transform HitPointHorizontal;

        bool readyToDig;
        bool canDig = true;
        float charging = 0;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            if (OnDigEvent == null)
                OnDigEvent = new UnityEvent();
            if (DigSuccessEvent == null)
                DigSuccessEvent = new UnityEvent();
            if (DiggingEvent == null)
                DiggingEvent = new UnityEvent<float, float>();
        }
        void Update()
        {
            direction = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                readyToDig = true;
                GetComponent<Animator>().SetBool("dig", true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                readyToDig = false;
                GetComponent<Animator>().SetBool("dig", false);
            }
        }
        private void FixedUpdate()
        {
            var mapManager = MapManager.Instance;

            if (readyToDig && canDig)
            {
                Vector3Int po1;
                Vector3Int po2;
                if (direction == 0)
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
            canDig = true;
            charging = 0;
            OnDigEvent.Invoke();
        }
        IEnumerator PA(Vector3Int point1, Vector3Int point2)
        {
            canDig = false;

            MapManager Ground = GameObject.Find("Map Manager").GetComponent<MapManager>();
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
                while (readyToDig && (time1 > charging || time2 > charging))
                {
                    //Debug.Log("굴착중!");
                    if (direction == 0)
                    {
                        currentPos1 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position + new Vector3(0.3f, 0, 0));
                        currentPos2 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointDown.position - new Vector3(0.3f, 0, 0));
                    }
                    else
                    {
                        currentPos1 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position + new Vector3(0, 0.3f, 0));
                        currentPos2 = Ground.groundTilemap.layoutGrid.WorldToCell(HitPointHorizontal.position - new Vector3(0, 0.3f, 0));
                    }

                    charging += Time.fixedDeltaTime;
                    float time = time1 >= time2 ? time1 : time2;
                    DiggingEvent.Invoke(charging, time);

                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                    // 파던 위치가 달라지면 초기화 및 탈출
                    if ((currentPos1 != point1) || (currentPos2 != point2))
                    {
                        Debug.Log("파던 위치가 달라졌잖아! 취소!");
                        ICanDig();
                        yield break;
                    }
                }
                if (time1 <= charging)
                {
                    Ground.BreakTile(point1);
                    player.status.hungry -= 5;
                    player.hungry_alter.Invoke(player.status.hungry, player.status.max_hungry);
                }
                if (time2 <= charging)
                {
                    Ground.BreakTile(point2);
                    player.status.hungry -= 5;
                    player.hungry_alter.Invoke(player.status.hungry, player.status.max_hungry);
                }
                ICanDig();
            }
            else
                ICanDig();
            //Debug.Log("없어!");
        }
    }
}