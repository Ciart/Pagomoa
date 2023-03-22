using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Dig : MonoBehaviour
{
    private PlayerController player;
    public float direction;
    public bool DiggingDown;
    public bool DiggingHorizontal;

    public float digSpeed = 10f;

    [SerializeField] public Transform HitPointDown;
    [SerializeField] public Transform HitPointHorizontal;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(direction == 0)
                DiggingDown = true;
            
            else if(direction != 0)
                DiggingHorizontal = true;
        }
    }
    void FixedUpdate()
    {
        if (DiggingDown)
        {
            _Dig();
            DiggingDown = false;
        }
        else if (DiggingHorizontal)
        {
            _Dig();
            DiggingHorizontal = false;
        }
        
    }
    public void _Dig()
    {
        Brick Ground = GameObject.Find("Grid").GetComponent<Brick>();

        bool CheckDig = false;
        if (DiggingDown)
        {
            Vector3 SizeL = new Vector3(HitPointDown.position.x - 0.3f, HitPointDown.position.y, HitPointDown.position.z);
            Vector3 SizeR = new Vector3(HitPointDown.position.x + 0.3f, HitPointDown.position.y, HitPointDown.position.z);
            StartCoroutine("_Digging", SizeL);
            StartCoroutine("_Digging", SizeR);
            //Ground.MakeDot(SizeL);
            //Ground.MakeDot(SizeR);
            CheckDig = true;
        }
        else if (DiggingHorizontal)
        {
            Vector3 SizeU = new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y + 0.3f, HitPointHorizontal.position.z);
            Vector3 SizeD = new Vector3(HitPointHorizontal.position.x, HitPointHorizontal.position.y - 0.3f, HitPointHorizontal.position.z);
            StartCoroutine("_Digging", SizeU);
            StartCoroutine("_Digging", SizeD);
            //Ground.MakeDot(SizeU);
            //Ground.MakeDot(SizeD);
            CheckDig = true;
        }

        if (CheckDig)
        {
            float hungrydeclineSpeed = 5;
            player.status.hungry -= hungrydeclineSpeed;
            player.hungry_alter.Invoke();
        }
    }
    IEnumerator _Digging(Vector3 point)
    {
        Brick Ground = GameObject.Find("Grid").GetComponent<Brick>();

        var tile = Ground.GetTile(Ground.groundTilemap.layoutGrid.WorldToCell(point));
        if (!tile) yield break;
        float spendTIme = tile.strength / digSpeed;

        Debug.Log($"광물 굴착시간: {spendTIme}");
        yield return new WaitForSeconds(spendTIme);
        Ground.MakeDot(point);
    }
}
