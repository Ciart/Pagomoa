using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class BrickSearcher : MonoBehaviour
    {
        public int searchRange = 15;
        public int minSearchRange = 0;

        public List<Vector2Int> onGroundList = new List<Vector2Int>();
        
        public Vector3 testPos = Vector3.zero;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                var x = Mathf.FloorToInt(pos.x);
                var y = Mathf.FloorToInt(pos.y);
                
                var intPos = new Vector2Int(x, y);
                
                // todo : gizmos용 제거 필요
                testPos = new Vector3(x + 0.5f, y + 0.5f, 0);

                GetAboveEmptyGroundVectors(intPos.x, intPos.y);
            }
        }

        public bool IsBrickAboveGround(int targetX, int targetY)
        {
            var world = WorldManager.instance.world;
            
            var targetBrick = world.GetBrick(targetX, targetY, out var notUseChunk1);

            if (targetBrick.ground) return false;

            var targetGroundBrick = world.GetBrick(targetX, targetY - 1, out var notUseChunk2);

            return targetGroundBrick.ground;
        }

        public void GetClosestAboveEmptyGroundVector(float basePosX, float basePosY)
        {
            var searchCount = 0;

            while (searchCount < searchRange - minSearchRange)
            {
                
            }
        }

        public List<Vector2Int> GetAboveEmptyGroundVectors(int basePosX, int basePosY)
        {
            onGroundList.Clear();

            var xRange = 0;
            var yRange = 0;
            
            xRange = basePosX + searchRange;
            yRange = basePosY - searchRange;

            for (var x = basePosX + 1; x < xRange; x++)
            {
                for (var y = basePosY; y > yRange; y--)
                {
                    if (x <= basePosX + minSearchRange && y >= basePosY - minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            xRange = basePosX - searchRange;
            yRange = basePosY + searchRange;
            
            for (var x = basePosX - 1; x > xRange; x--)
            {
                for (var y = basePosY; y < yRange; y++)
                {
                    if (x >= basePosX - minSearchRange && y <= basePosY + minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            xRange = basePosX + searchRange;
            yRange = basePosY + searchRange;
            
            for (var y = basePosY + 1; y < yRange; y++)
            {
                for (var x = basePosX; x < xRange; x++)
                {
                    if (x <= basePosX + minSearchRange && y <= basePosY + minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            xRange = basePosX - searchRange;
            yRange = basePosY - searchRange;
            
            for (var y = basePosY - 1; y > yRange; y--)
            {
                for (var x = basePosX; x > xRange; x--)
                {
                    if (x >= basePosX - minSearchRange && y >= basePosY - minSearchRange) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            return onGroundList;
        }

        private void OnDrawGizmos()
        {
            if (testPos == Vector3.zero) return;
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawCube(testPos, new Vector3(0.5f,0.5f));

            Gizmos.DrawWireCube(testPos, new Vector3(searchRange * 2, searchRange * 2));
            
            if (minSearchRange != 0) Gizmos.DrawWireCube(testPos, new Vector3(minSearchRange * 2, minSearchRange * 2));
            
            
            foreach (var intPos in onGroundList)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(new Vector3(intPos.x, intPos.y), new Vector3(intPos.x + 1, intPos.y));
                Gizmos.DrawCube(new Vector3(intPos.x + 0.5f, intPos.y + 0.5f), new Vector3(0.5f,0.5f));
            }
        }
    }
}
