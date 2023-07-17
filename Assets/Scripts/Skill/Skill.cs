using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using Player;
using Tiles;
using Constants;

public class Skill : MonoBehaviour
{
    PlayerController _player;
    bool Helicopter = false;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Helicopter = !Helicopter;
            if (Helicopter)
                StartCoroutine(HelicopterStart());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            MoleHill();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GoMole());
        }
    }
    
    // Skill : 지하에 있을때 지상까지 헬리콥터를 운행합니다.    
    // Motion: 머리위 작은 헬리콥터 혹은 드릴을 머리위로 잡고 날아가는 모션
    IEnumerator HelicopterStart()
    {
        Debug.Log("빙글빙글 비틀비틀");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (MapManager.Instance.CheckClimbable(transform.position) && Helicopter)
        {
            _player.state = Player.PlayerState.Jump;
            if(rb.velocity.y >= 0)
                rb.AddForce(new Vector2(0, 30), ForceMode2D.Force);
            else
                rb.AddForce(new Vector2(0, 60), ForceMode2D.Force);
            yield return new WaitForFixedUpdate();
        }
        Helicopter = false;
        Debug.Log("콥터 종료");
    }
    // Skill : 광물을 하나 발견할 때까지 수직 아래 방향으로 땅을 한번에 팝니다. 
    // Motion: 엑스칼리버 꽂듯 드릴을 땅에 내리꽂는 모션, 혹은 두더지 수직 낙하 모션(다이빙하듯)
    // 
    void MoleHill()
    {
        Debug.Log("두더지구멍!");
        Vector3 digVec;
        switch (_player.GetDirection()) 
        {
            case Direction.Left:
                digVec = Vector3.left;
                break;
            case Direction.Right:
                digVec = Vector3.right;
                break;
            default:
                digVec = Vector3.down;
                break;
        }
        Vector3 point = transform.position + digVec * 1.2f;
        Vector3Int pointInt = MapManager.Instance.groundTilemap.layoutGrid.WorldToCell(point);
        while (MapManager.Instance.GetBrick(pointInt).ground)
        {
            if (MapManager.Instance.mineralTilemap.GetTile<MineralTile>(pointInt))
                return;
            MapManager.Instance.BreakTile(pointInt, 99999);
            point = point + digVec;
            pointInt = MapManager.Instance.groundTilemap.layoutGrid.WorldToCell(point);
        }
    }
    // Skill : 광물을 하나 발견할 때까지 무작위 방향의 땅을 차근차근 팝니다.  
    // Motion: 손으로 바닥을 대각선으로 향해 가리키며 두더지가 바닥에 뛰어드는 모션
    IEnumerator GoMole()
    {
        Debug.Log("가라 두더지!!");
        Vector3 point = transform.position + new Vector3(0, -1.2f);
        Vector3Int pointInt = MapManager.Instance.groundTilemap.layoutGrid.WorldToCell(point);
        bool find = false;

        while (MapManager.Instance.GetBrick(pointInt).ground && !find)
        {
            if (MapManager.Instance.mineralTilemap.GetTile<MineralTile>(pointInt))
                find = true;
            MapManager.Instance.BreakTile(pointInt, 99999);
            switch(Random.Range(0, 3))
            {
                case 0:
                    point += Vector3.right;
                    break;
                case 1:
                    point += Vector3.left;
                    break;
                case 2:
                    point += Vector3.down;
                    break;
                case 3:
                    point += Vector3.up;
                    break;
            }
            pointInt = MapManager.Instance.groundTilemap.layoutGrid.WorldToCell(point);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
