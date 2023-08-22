using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Worlds;

namespace UFO
{
    public class UFOLadderInteraction : MonoBehaviour
    {
        private Transform _floorTransform;

        private TilemapCollider2D _floorTilemapCollider;
        
        private Tilemap _ladder;

        private void Start()
        {
            _floorTransform = transform.parent.Find("SecondFloor");
            _floorTilemapCollider = _floorTransform.GetComponent<TilemapCollider2D>();
            
            _ladder = GetComponent<Tilemap>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.name == "Player")
            {
                var playerPos = collision.transform.position;
                var coords = WorldManager.ComputeCoords(new Vector3(playerPos.x, playerPos.y));
                var ladderCellPos = _ladder.WorldToCell(new Vector3Int(coords.x, coords.y - 1));
                
                if (_ladder.HasTile(ladderCellPos))
                {
                    if (collision.GetComponent<PlayerMovement>().isClimb)
                    {
                        Physics2D.IgnoreCollision( collision.GetComponent<BoxCollider2D>(), _floorTilemapCollider, true);
                    } else if (!collision.GetComponent<PlayerMovement>().isClimb)
                    {
                        Physics2D.IgnoreCollision( collision.GetComponent<BoxCollider2D>(), _floorTilemapCollider, false);
                    }    
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Player")
            {
                Physics2D.IgnoreCollision( collision.GetComponent<BoxCollider2D>(), _floorTilemapCollider, false);
            }
        }
    }
}
