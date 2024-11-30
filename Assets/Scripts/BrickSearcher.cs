using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public List<Vector2Int> closeBricks = new List<Vector2Int>();
        
        public Vector3 testPos = Vector3.zero;

        public Vector2Int targetVector;

        public Vector2Int targetBrickVector2Int;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                { 
                    var x = Mathf.FloorToInt(pos.x);
                var y = Mathf.FloorToInt(pos.y);
                
                var intPos = new Vector2Int(x, y);
                
                // todo : gizmos용 제거 필요
                testPos = new Vector3(x + 0.5f, y + 0.5f, 0);

                GetAboveEmptyGroundVectors(intPos.x, intPos.y);    
                }

                /*{
                    testPos = new Vector3(pos.x, pos.y, 0);
                    
                    GetClosestAboveEmptyGroundVector(pos.x, pos.y);
                    
                }*/
            }
        }

        public bool IsBrickAboveGround(int targetX, int targetY)
        {
            var lev = WorldManager.world.currentLevel;
            
            var targetBrick = lev.GetBrick(targetX, targetY, out var notUseChunk1);

            if (targetBrick.ground != null) return false;

            var targetGroundBrick = lev.GetBrick(targetX, targetY - 1, out var notUseChunk2);

            return targetGroundBrick.ground != null;
        }

        public Vector2Int GetClosestAboveEmptyGroundVector(float basePosX, float basePosY)
        {
            closeBricks.Clear();
            
            var searchCount = 0;

            var x = Mathf.FloorToInt(basePosX);
            var y = Mathf.FloorToInt(basePosY);

            var initVector = new Vector2Int(-1, 1);
            var intPos = new Vector2Int(x, y);

            var xPlusCount = 2 + 2 * minSearchRange;
            var xMinusCount = -2 - 2 * minSearchRange;
            var yPlusCount = 2 + 2 * minSearchRange;
            var yMinusCount = -2 - 2 * minSearchRange;
            
            var startPos = intPos + initVector * minSearchRange;

            while (searchCount < searchRange - minSearchRange)
            {
                startPos += initVector ;

                targetVector = startPos;

                for (int xp = 0; xp < xPlusCount; xp++)
                {
                    targetVector += new Vector2Int(1, 0);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                for (int ym = 0; ym > yMinusCount; ym--)
                {
                    targetVector += new Vector2Int(0, -1);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }

                for (int xm = 0; xm > xMinusCount; xm--)
                {
                    targetVector += new Vector2Int(-1, 0);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }
                
                for (int yp = 0; yp < yPlusCount; yp++)
                {
                    targetVector += new Vector2Int(0, 1);

                    if (IsBrickAboveGround(targetVector.x, targetVector.y))
                    {
                        var targetBrick = new Vector2Int(targetVector.x, targetVector.y);
                        closeBricks.Add(targetBrick);
                    }
                }
                
                if (closeBricks.Count > 0)
                {
                    var targetPos = new Vector2(basePosX, basePosY);
                    var distances = new Dictionary<float, Vector2Int>(); 
                    
                    foreach (var vector in closeBricks)
                    {
                        var distance = Vector2.Distance(targetPos, vector);
                        distances.Add(distance, vector);
                    }
                    
                    var minDistance = distances.Keys.Min();

                    // todo : 지워
                    targetBrickVector2Int = distances[minDistance]; 
                    
                    return distances[minDistance];
                }

                searchCount++;
                
                xPlusCount += 2;
                xMinusCount -= 2;
                yPlusCount += 2;
                yMinusCount -= 2;
            }
            
            // 주변에 가까운 블럭이 없으면 매개변수를 다시 반환
            return new Vector2Int((int)basePosX, (int)basePosY);
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

            Gizmos.color = Color.green;

            foreach (var brick in closeBricks)
            {
                Gizmos.DrawCube(new Vector3(brick.x + 0.5f, brick.y + 0.5f), new Vector3(0.5f,0.5f));    
            }

            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(targetBrickVector2Int.x + 0.5f, targetBrickVector2Int.y + 0.5f), new Vector3(0.3f, 0.3f));
        }
    }
}
