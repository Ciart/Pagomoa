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
    
    // Skill : ���Ͽ� ������ ������� �︮���͸� �����մϴ�.    
    // Motion: �Ӹ��� ���� �︮���� Ȥ�� �帱�� �Ӹ����� ��� ���ư��� ���
    IEnumerator HelicopterStart()
    {
        Debug.Log("���ۺ��� ��Ʋ��Ʋ");
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
        Debug.Log("���� ����");
    }
    // Skill : ������ �ϳ� �߰��� ������ ���� �Ʒ� �������� ���� �ѹ��� �˴ϴ�. 
    // Motion: ����Į���� �ȵ� �帱�� ���� �����ȴ� ���, Ȥ�� �δ��� ���� ���� ���(���̺��ϵ�)
    // 
    void MoleHill()
    {
        Debug.Log("�δ�������!");
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
    // Skill : ������ �ϳ� �߰��� ������ ������ ������ ���� �������� �˴ϴ�.  
    // Motion: ������ �ٴ��� �밢������ ���� ����Ű�� �δ����� �ٴڿ� �پ��� ���
    IEnumerator GoMole()
    {
        Debug.Log("���� �δ���!!");
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
