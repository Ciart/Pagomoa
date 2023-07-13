using System;
using System.Collections;
using System.Collections.Generic;
using Maps;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NightMonsterSpawner : MonoBehaviour
{
    public Transform player;
    
    public bool x, y, z;
    
    public int land;

    [SerializeField] private int _maxSpawn = 10;
    
    [SerializeField] private float _spawnTerm = 30f;

    [SerializeField] private GameObject[] _monsterType;
    
    private CircleCollider2D _circleCollider2D;
    
    private GameObject _monsterPrefab;

    private List<GameObject> _spawnedMonster;

    private Vector3Int _spawnPoint;

    private float _desertDepth = 50f;
    private float _forestDepth = -100f;

    private int _boxSize = 15;
    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spawnedMonster = new List<GameObject>();

        SetSpawnMonster();
        CheckSpawnPosition();
    }

    void FixedUpdate()
    {
        ChasePlayer();
        if (_spawnTerm <= 0)
        {
            _spawnTerm = 10f;
        }
        if (_spawnTerm == 10f)
        {
            StartCoroutine(nameof(SpawnMonsters));
        }
        if (_spawnTerm <= 10f)
        {
            _spawnTerm -= 0.1f;
        }
    }

    private void ChasePlayer()
    {
        if (!player) return ;
        
        transform.position = new Vector3(
            (x ? player.position.x : transform.position.x),
            (y ? player.position.y : transform.position.y),
            (z ? player.position.z : transform.position.z)); 
    }

    private IEnumerator SpawnMonsters()
    {
        if (_spawnedMonster.Count < _maxSpawn)
        {
            
            
            GameObject nightMonster = Instantiate(_monsterPrefab, _spawnPoint, Quaternion.identity);
            _spawnedMonster.Add(nightMonster);
            yield return new WaitForSeconds(_spawnTerm);
        }
        yield return null;
        SetSpawnMonster();
    }
    
    private void SetSpawnMonster()
    {
        if (player.transform.position.y > _desertDepth)
        {
            land = 1;
            _monsterPrefab = _monsterType[land-1];
        } else if (player.transform.position.y > _forestDepth)
        {
            land = 2;
            _monsterPrefab = _monsterType[land-1];
        }
    }

    private void CheckSpawnPosition()
    {
        var mapManager = MapManager.Instance;
        
        Vector3Int bottomLeft = new Vector3Int(
            Mathf.FloorToInt(player.position.x) - _boxSize / 2,
            Mathf.FloorToInt(player.position.y) - _boxSize / 2,
            Mathf.FloorToInt(player.position.z)
        );
        Vector3Int topRight = new Vector3Int(
            Mathf.CeilToInt(player.position.x) + _boxSize / 2 - 1,
            Mathf.CeilToInt(player.position.y) + _boxSize / 2 - 1,
            Mathf.FloorToInt(player.position.z)
        );
        
        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                Vector3Int tilePosition = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3Int(x, y, bottomLeft.z));
                var tile = mapManager.GetBrick(tilePosition).ground;

                if (tile)
                {
                    Vector3Int isNullTilePos = mapManager.groundTilemap.layoutGrid.WorldToCell(new Vector3Int(x, y + 1, bottomLeft.z));
                    var tileCheck= mapManager.GetBrick(isNullTilePos).ground;

                    if (!tileCheck)
                    {
                        _spawnPoint = isNullTilePos;
                    }
                }
            }
        }
    }
}
