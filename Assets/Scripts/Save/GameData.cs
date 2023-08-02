using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worlds;


public class GameData
{
    public WorldData worldData;
    public PositionData posData;
}
[System.Serializable]
public class PositionData
{
    public DicList<string, Vector3> positionData;
    public void SetPositionDataFromDictionary(Dictionary<string, Vector3> dic)
    {
        positionData = ListDictionaryConverter.ToList(dic);
    }
}
[System.Serializable]
public class WorldData
{
    public int chunkSize;

    public int top;

    public int bottom;

    public int left;

    public int right;

    public int groundHeight = 0;

    public DicList<Vector2Int, Chunk> _chunks;

    public void SetWorldDataFromWorld(World world)
    {
        chunkSize = world.chunkSize;
        top = world.top;
        bottom = world.bottom;
        left = world.left;
        right = world.right;
        groundHeight = world.groundHeight;
        _chunks = ListDictionaryConverter.ToList(world.GetAllChunks(), true);
    }
}