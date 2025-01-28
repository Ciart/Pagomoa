using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public static class BrickSearchUtility
    {
        private static bool IsBrickAboveGround(int targetX, int targetY)
        {
            var lev = WorldManager.world.currentLevel;
            
            var targetBrick = lev.GetBrick(targetX, targetY, out var notUseChunk1);

            if (targetBrick.ground != null) return false;

            var targetGroundBrick = lev.GetBrick(targetX, targetY - 1, out var notUseChunk2);

            return targetGroundBrick.ground != null;
        }

        public static List<Vector2Int> GetAboveEmptyGroundVectors(Vector2Int basePoint, Vector2Int searchRange, Vector2Int searchExclusionRange)
        {
            List<Vector2Int> onGroundList = new List<Vector2Int>();

            var xRange = 0;
            var yRange = 0;
            
            xRange = basePoint.x + searchRange.x;
            yRange = basePoint.y - searchRange.y;

            for (var x = basePoint.x + 1; x < xRange; x++)
            {
                for (var y = basePoint.y; y > yRange; y--)
                {
                    if (x <= basePoint.x + searchExclusionRange.x && y >= basePoint.y - searchExclusionRange.y) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            xRange = basePoint.x - searchRange.x;
            yRange = basePoint.y + searchRange.y;
            
            for (var x = basePoint.x - 1; x > xRange; x--)
            {
                for (var y = basePoint.y; y < yRange; y++)
                {
                    if (x >= basePoint.x - searchExclusionRange.x && y <= basePoint.y + searchExclusionRange.y) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }

            xRange = basePoint.x + searchRange.x;
            yRange = basePoint.y + searchRange.y;
            
            for (var y = basePoint.y + 1; y < yRange; y++)
            {
                for (var x = basePoint.x; x < xRange; x++)
                {
                    if (x <= basePoint.x + searchExclusionRange.x && y <= basePoint.y + searchExclusionRange.y) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            xRange = basePoint.x - searchRange.x;
            yRange = basePoint.y - searchRange.y;
            
            for (var y = basePoint.y - 1; y > yRange; y--)
            {
                for (var x = basePoint.x; x > xRange; x--)
                {
                    if (x >= basePoint.x - searchExclusionRange.x && y >= basePoint.y - searchExclusionRange.y) continue;
                    if (!IsBrickAboveGround(x, y)) continue;
                    
                    var targetIntPos = new Vector2Int(x, y);
                    onGroundList.Add(targetIntPos);
                }
            }
            
            return onGroundList;
        }
    }
}
