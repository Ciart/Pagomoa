using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class DrawPastWay : MonoBehaviour
{
    public BoxCollider2D sightRange;
    public LayerMask whatisPlatform;
    void Start()
    {
        sightRange = GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {
        LightBlocks();
    }
    void LightBlocks()
    {
        int halfSizeX = Mathf.RoundToInt(sightRange.size.x/2);
        for(int i = -halfSizeX; i <= halfSizeX; i++)
        {
            for(int j = -halfSizeX; j <= halfSizeX; j++)
            {
                Vector3 checkCellPos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                float distance = Vector2.Distance(transform.position, checkCellPos) - 0.001f;

                if (distance <= halfSizeX)
                {
                    Collider2D overCol = Physics2D.OverlapBox(checkCellPos, new Vector2(halfSizeX, halfSizeX), 0, whatisPlatform);
                    overCol.transform.GetComponent<SetLight>().SetTilmapLightly(checkCellPos);
                }
            }
        }
    }
}
