using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class Dig : MonoBehaviour
{
    public UnityEvent OnDigEvent;

    public floatfloatEvent DiggingEvent;

    [System.Serializable]
    public class floatfloatEvent : UnityEvent<float, float> { }

    private PlayerController player;
    public float direction;
    public float digSpeed = 10f;

    public bool Digging;
    bool isDigging;

    [SerializeField] public Transform HitPointDown;
    [SerializeField] public Transform HitPointHorizontal;


    public float DigHoldTIme = 0;
    public float StandTime = 0;

    Vector3Int DigSpot1 = Vector3Int.zero;
    Vector3Int DigSpot2 = Vector3Int.zero;


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        if (OnDigEvent == null)
            OnDigEvent = new UnityEvent();
        if (DiggingEvent == null)
            DiggingEvent = new floatfloatEvent();
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            isDigging = true;
        if (Input.GetKeyUp(KeyCode.Space))
            isDigging = false;

        // 땅파기 최초시작시 실행할 코드
    }
    void FixedUpdate()
    {
        if (isDigging)
        {
            Brick Ground = GameObject.Find("Grid").GetComponent<Brick>();
            Debug.Log("굴착시작! ");
            if (direction == 0)
            {
                DigSpot1 = Ground.groundTilemap.layoutGrid.WorldToCell(new Vector2(HitPointDown.position.x - 0.3f, HitPointDown.position.y));
                DigSpot2 = Ground.groundTilemap.layoutGrid.WorldToCell(new Vector2(HitPointDown.position.x + 0.3f, HitPointDown.position.y));
            }
            else
            {
                DigSpot1 = Ground.groundTilemap.layoutGrid.WorldToCell(new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y + 0.3f));
                DigSpot2 = Ground.groundTilemap.layoutGrid.WorldToCell(new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y - 0.3f));
            }
            Digging = true;
        }
        else
        {
            Digging = false;
            DigHoldTIme = 0;
        }


        if (Digging)
        {
            _Dig();
            
        }
        else
        {
            DigHoldTIme = 0;
            OnDigEvent.Invoke();

        }

    }
    public void _Dig()
    {
        DiggingEvent.Invoke(DigHoldTIme, StandTime);
        Brick Ground = GameObject.Find("Grid").GetComponent<Brick>();

        var tile1 = Ground.GetTile(DigSpot1);
        var tile2 = Ground.GetTile(DigSpot2);
        if (!(tile1 || tile2))
        {
            Digging = false;
            DigHoldTIme = 0;
            return;
        }
        DigHoldTIme += Time.fixedDeltaTime;
        bool CheckDig = false;
        float spendTIme;
        if (tile1)
        {
            spendTIme = tile1.strength / digSpeed;
            StandTime = spendTIme;
            Debug.Log($"광물1 굴착시간: {spendTIme}");
            if (spendTIme <= DigHoldTIme)
            {
                Ground.MakeDot(DigSpot1);
                CheckDig = true;
            }
        }
        if (tile2)
        {
            spendTIme = tile2.strength / digSpeed;
            StandTime = spendTIme;

            Debug.Log($"광물2 굴착시간: {spendTIme}");
            if (spendTIme <= DigHoldTIme)
            {
                Ground.MakeDot(DigSpot2);
                CheckDig = true;
            }
        }
        if (CheckDig && (!Ground.GetTile(DigSpot1))&& (!Ground.GetTile(DigSpot2)))
        {
            DigHoldTIme = 0;
            isDigging = true;
            float hungrydeclineSpeed = 5;
            player.status.hungry -= hungrydeclineSpeed;
            player.hungry_alter.Invoke(player.status.hungry, player.status.max_hungry);
            //Debug.Log("초기화" + Digging);
        }
    }
}
